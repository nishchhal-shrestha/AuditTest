using Abp.Application.Services;
using AuditTest.MultiTenancy.Dto;

namespace AuditTest.MultiTenancy
{
    public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, PagedTenantResultRequestDto, CreateTenantDto, TenantDto>
    {
    }
}

