using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.EntityHistory;
using Abp.Events.Bus.Entities;
using Abp.Extensions;
using Abp.Json;
using Abp.Reflection;
using Abp.Timing;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace AuditTest.CustomEntityHelper
{
    public class CustomEntityHistoryHelper : EntityHistoryHelperBase, ICustomEntityHistoryHelper, ITransientDependency
    {
        public CustomEntityHistoryHelper(
            IEntityHistoryConfiguration configuration,
            IUnitOfWorkManager unitOfWorkManager)
            : base(configuration, unitOfWorkManager)
        {
            
        }

        public async Task SaveAsync(EntityChangeSet changeSet)
        {
            if (!IsEntityHistoryEnabled)
            {
                return;
            }

            if (changeSet.EntityChanges.Count == 0)
            {
                return;
            }

            using (var uow = UnitOfWorkManager.Begin(TransactionScopeOption.Suppress))
            {
                await EntityHistoryStore.SaveAsync(changeSet);
                await uow.CompleteAsync();
            }
        }

        public void Save(EntityChangeSet changeSet)
        {
            if (!IsEntityHistoryEnabled)
            {
                return;
            }

            if (changeSet.EntityChanges.Count == 0)
            {
                return;
            }

            using (var uow = UnitOfWorkManager.Begin(TransactionScopeOption.Suppress))
            {
                EntityHistoryStore.Save(changeSet);
                uow.Complete();
            }
        }

        public EntityChangeSet CreateEntityChangeSet(ICollection<CustomEntityEntry<CustomEntity>> entityEntries)
        {
            var changeSet = new EntityChangeSet
            {
                Reason = EntityChangeSetReasonProvider.Reason.TruncateWithPostfix(EntityChangeSet.MaxReasonLength),

                // Fill "who did this change"
                BrowserInfo = ClientInfoProvider.BrowserInfo.TruncateWithPostfix(EntityChangeSet.MaxBrowserInfoLength),
                ClientIpAddress =
                    ClientInfoProvider.ClientIpAddress.TruncateWithPostfix(EntityChangeSet.MaxClientIpAddressLength),
                ClientName = ClientInfoProvider.ComputerName.TruncateWithPostfix(EntityChangeSet.MaxClientNameLength),
                ImpersonatorTenantId = AbpSession.ImpersonatorTenantId,
                ImpersonatorUserId = AbpSession.ImpersonatorUserId,
                TenantId = AbpSession.TenantId,
                UserId = AbpSession.UserId
            };

            if (!IsEntityHistoryEnabled)
            {
                return changeSet;
            }

            foreach (var entityEntry in entityEntries)
            {
                var shouldSaveEntityHistory = ShouldSaveEntityHistory(entityEntry);
                if (shouldSaveEntityHistory.HasValue && !shouldSaveEntityHistory.Value)
                {
                    continue;
                }

                var entityChange = CreateEntityChange(entityEntry);
                if (entityChange == null)
                {
                    continue;
                }

                var shouldSaveAuditedPropertiesOnly = !shouldSaveEntityHistory.HasValue;
                var propertyChanges = GetPropertyChanges(entityEntry, shouldSaveAuditedPropertiesOnly);
                if (propertyChanges.Count == 0)
                {
                    continue;
                }

                entityChange.PropertyChanges = propertyChanges;
                changeSet.EntityChanges.Add(entityChange);
            }

            return changeSet;
        }

        protected virtual bool? ShouldSaveEntityHistory(CustomEntityEntry<CustomEntity> entityEntry)
        {            
            Type typeOfEntity;

            if (entityEntry.oldValue != null) 
            {
                typeOfEntity = entityEntry.oldValue.GetType();
            } 
            else
            {
                typeOfEntity= entityEntry.newValue.GetType();
            }

            var shouldTrackEntity = IsTypeOfTrackedEntity(typeOfEntity);


            if (shouldTrackEntity.HasValue && !shouldTrackEntity.Value)
            {
                return false;
            }

            if (!IsTypeOfEntity(typeOfEntity))
            {
                return false;
            }

            var shouldAuditEntity = IsTypeOfAuditedEntity(typeOfEntity);
            if (shouldAuditEntity.HasValue && !shouldAuditEntity.Value)
            {
                return false;
            }

            return shouldAuditEntity ?? shouldTrackEntity;
        }

        protected virtual bool ShouldSavePropertyHistory(CustomEntityEntry<CustomEntity> entityEntry, PropertyInfo propertyInfo, bool defaultValue)
        {
            if (propertyInfo == null) // Shadow properties or if mapped directly to a field
            {
                return defaultValue;
            }

            return IsAuditedPropertyInfo(propertyInfo) ?? defaultValue;
        }

        [CanBeNull]
        private EntityChange CreateEntityChange(CustomEntityEntry<CustomEntity> entityEntry)
        {
            string entityId;
            string entityTypeFullName;

            if(entityEntry.oldValue == null)
            {
                entityId = entityEntry.newValue.Id.ToString();
                entityTypeFullName = entityEntry.newValue.GetType().FullName;
            }
            else 
            {
                entityId = entityEntry.oldValue.Id.ToString();
                entityTypeFullName = entityEntry.oldValue.GetType().FullName;
            }

            EntityChangeType changeType = entityEntry.entityChangeType;

            return new EntityChange
            {
                ChangeType = changeType,
                EntityEntry = entityEntry, // [NotMapped]
                EntityId = entityId,
                EntityTypeFullName = entityTypeFullName,
                TenantId = AbpSession.TenantId,
                ChangeTime = Clock.Now                
            };
        }

        private ICollection<EntityPropertyChange> GetPropertyChanges(CustomEntityEntry<CustomEntity> entityEntry,
            bool auditedPropertiesOnly)
        {
            var propertyChanges = new List<EntityPropertyChange>();
            PropertyInfo[] properties;
            if (entityEntry.oldValue == null)
            {
                properties = entityEntry.newValue.GetType().GetProperties();
            }
            else
            {
                properties = entityEntry.oldValue.GetType().GetProperties();
            }

            foreach (var property in properties)
            {
                if (ShouldSavePropertyHistory(entityEntry, property, !auditedPropertiesOnly))
                {
                    var oldValue = entityEntry.oldValue == null ? null : property.GetValue(entityEntry.oldValue);
                    var newValue = entityEntry.newValue == null ? null : property.GetValue(entityEntry.newValue);

                    var entityPropertyChange = CreateEntityPropertyChange(
                            oldValue,
                            newValue,
                            property
                        );

                    if (entityPropertyChange.NewValueHash != entityPropertyChange.OriginalValueHash)
                    {
                        propertyChanges.Add(
                            entityPropertyChange
                        );
                    }
                }
            }

            return propertyChanges;
        }


        private EntityPropertyChange CreateEntityPropertyChange(object oldValue, object newValue, PropertyInfo property)
        {
            var entityPropertyChange = new EntityPropertyChange()
            {
                PropertyName = property.Name.TruncateWithPostfix(EntityPropertyChange.MaxPropertyNameLength),
                PropertyTypeFullName = property.PropertyType.FullName.TruncateWithPostfix(
                    EntityPropertyChange.MaxPropertyTypeFullNameLength
                ),
                TenantId = AbpSession.TenantId
            };

            entityPropertyChange.SetNewValue(newValue?.ToJsonString());
            entityPropertyChange.SetOriginalValue(oldValue?.ToJsonString());
            return entityPropertyChange;
        }
    }
}
