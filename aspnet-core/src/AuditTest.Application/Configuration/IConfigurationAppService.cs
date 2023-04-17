using System.Threading.Tasks;
using AuditTest.Configuration.Dto;

namespace AuditTest.Configuration
{
    public interface IConfigurationAppService
    {
        Task ChangeUiTheme(ChangeUiThemeInput input);
    }
}
