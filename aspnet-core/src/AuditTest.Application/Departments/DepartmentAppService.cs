using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.EntityHistory;
using Abp.Events.Bus.Entities;
using Abp.Extensions;
using Abp.IdentityFramework;
using Abp.Linq.Extensions;
using Abp.MultiTenancy;
using Abp.Reflection;
using Abp.Runtime.Security;
using AuditTest.Authorization;
using AuditTest.Authorization.Roles;
using AuditTest.Authorization.Users;
using AuditTest.CustomEntityHelper;
using AuditTest.Departments.Dto;
using AuditTest.Editions;
using AuditTest.Entity;
using AuditTest.MultiTenancy;
using AuditTest.MultiTenancy.Dto;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using static AuditTest.Authorization.Roles.StaticRoleNames;

namespace AuditTest.Departments
{
    [AbpAuthorize]
    public class DepartmentAppService : AsyncCrudAppService<Department, DepartmentDto, Guid, PagedResultRequestDto, CreateDepartmentDto, UpdateDepartmentDto>, IDepartmentAppService
    {
        private readonly ICustomEntityHistoryHelper _customEntityHistoryHelper;

        public DepartmentAppService(IRepository<Department, Guid> departmentRepository, ICustomEntityHistoryHelper customEntityHistoryHelper)
            : base(departmentRepository)
        {
            _customEntityHistoryHelper = customEntityHistoryHelper;
        }

        public override async Task<DepartmentDto> CreateAsync(CreateDepartmentDto input)
        {           
            var departmentDto = await base.CreateAsync(input);

            var designation = new Designation() { 
                Id = departmentDto.Id.ToString(),
                Name = departmentDto.Name,
                CreationTime = departmentDto.CreationTime,
                CreatorUserId = departmentDto.CreatorUserId,
                DeleterUserId = departmentDto.DeleterUserId,
                DeletionTime = departmentDto.DeletionTime,
                IsActive = departmentDto.IsActive,
                IsDeleted = departmentDto.IsDeleted,
                LastModificationTime = departmentDto.LastModificationTime,
                LastModifierUserId = departmentDto.LastModifierUserId
            };

            var customEntityEntries = new List<CustomEntityEntry<CustomEntity>>();
            customEntityEntries.Add(new CustomEntityEntry<CustomEntity>(null, designation, EntityChangeType.Created));
            var changeSet = this._customEntityHistoryHelper.CreateEntityChangeSet(customEntityEntries);
            await this._customEntityHistoryHelper.SaveAsync(changeSet);

            return departmentDto;
        }

        public override async Task<DepartmentDto> UpdateAsync(UpdateDepartmentDto input)
        {
            var oldDepartmentDto = await base.GetAsync(new DepartmentDto { Id = input.Id });

            var oldDesignation = new Designation()
            {
                Id = oldDepartmentDto.Id.ToString(),
                Name = oldDepartmentDto.Name,
                CreationTime = oldDepartmentDto.CreationTime,
                CreatorUserId = oldDepartmentDto.CreatorUserId,
                DeleterUserId = oldDepartmentDto.DeleterUserId,
                DeletionTime = oldDepartmentDto.DeletionTime,
                IsActive = oldDepartmentDto.IsActive,
                IsDeleted = oldDepartmentDto.IsDeleted,
                LastModificationTime = oldDepartmentDto.LastModificationTime,
                LastModifierUserId = oldDepartmentDto.LastModifierUserId
            };

            var departmentDto = await base.UpdateAsync(input);

            var newDesignation = new Designation()
            {
                Id = departmentDto.Id.ToString(),
                Name = departmentDto.Name,
                CreationTime = departmentDto.CreationTime,
                CreatorUserId = departmentDto.CreatorUserId,
                DeleterUserId = departmentDto.DeleterUserId,
                DeletionTime = departmentDto.DeletionTime,
                IsActive = departmentDto.IsActive,
                IsDeleted = departmentDto.IsDeleted,
                LastModificationTime = departmentDto.LastModificationTime,
                LastModifierUserId = departmentDto.LastModifierUserId              
            };

            var customEntityEntries = new List<CustomEntityEntry<CustomEntity>>();
            customEntityEntries.Add(new CustomEntityEntry<CustomEntity>(oldDesignation, newDesignation, EntityChangeType.Updated));
            var changeSet = this._customEntityHistoryHelper.CreateEntityChangeSet(customEntityEntries);
            await this._customEntityHistoryHelper.SaveAsync(changeSet);

            return departmentDto;
        }

        public override async Task DeleteAsync(EntityDto<Guid> input)
        {
            var oldDepartmentDto = await base.GetAsync(new DepartmentDto { Id = input.Id });

            var oldDesignation = new Designation()
            {
                Id = oldDepartmentDto.Id.ToString(),
                Name = oldDepartmentDto.Name,
                CreationTime = oldDepartmentDto.CreationTime,
                CreatorUserId = oldDepartmentDto.CreatorUserId,
                DeleterUserId = oldDepartmentDto.DeleterUserId,
                DeletionTime = oldDepartmentDto.DeletionTime,
                IsActive = oldDepartmentDto.IsActive,
                IsDeleted = oldDepartmentDto.IsDeleted,
                LastModificationTime = oldDepartmentDto.LastModificationTime,
                LastModifierUserId = oldDepartmentDto.LastModifierUserId
            };

            await base.DeleteAsync(input);

            var customEntityEntries = new List<CustomEntityEntry<CustomEntity>>();
            customEntityEntries.Add(new CustomEntityEntry<CustomEntity>(oldDesignation, null, EntityChangeType.Deleted));
            var changeSet = this._customEntityHistoryHelper.CreateEntityChangeSet(customEntityEntries);
            await this._customEntityHistoryHelper.SaveAsync(changeSet);
        }
    }
}

