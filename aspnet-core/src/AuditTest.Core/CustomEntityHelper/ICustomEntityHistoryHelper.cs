using Abp.EntityHistory;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditTest.CustomEntityHelper
{
    public interface ICustomEntityHistoryHelper
    {
        public EntityChangeSet CreateEntityChangeSet(ICollection<CustomEntityEntry<CustomEntity>> entityEntries);

        Task SaveAsync(EntityChangeSet changeSet);

        void Save(EntityChangeSet changeSet);
    }
}
