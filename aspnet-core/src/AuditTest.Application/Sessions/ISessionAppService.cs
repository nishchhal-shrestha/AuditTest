using System.Threading.Tasks;
using Abp.Application.Services;
using AuditTest.Sessions.Dto;

namespace AuditTest.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}
