using System.Threading.Tasks;
using Abp.Application.Services;
using AuditTest.Authorization.Accounts.Dto;

namespace AuditTest.Authorization.Accounts
{
    public interface IAccountAppService : IApplicationService
    {
        Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input);

        Task<RegisterOutput> Register(RegisterInput input);
    }
}
