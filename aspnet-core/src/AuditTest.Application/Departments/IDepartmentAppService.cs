using Abp.Application.Services;
using Abp.Application.Services.Dto;
using AuditTest.Departments.Dto;
using AuditTest.MultiTenancy.Dto;
using System;
using System.Threading.Tasks;

namespace AuditTest.Departments
{
    public interface IDepartmentAppService : IAsyncCrudAppService<DepartmentDto, Guid, PagedResultRequestDto, CreateDepartmentDto, UpdateDepartmentDto>
    {
    }
}

