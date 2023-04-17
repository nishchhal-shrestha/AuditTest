using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Runtime.Session;
using AuditTest.Configuration.Dto;

namespace AuditTest.Configuration
{
    [AbpAuthorize]
    public class ConfigurationAppService : AuditTestAppServiceBase, IConfigurationAppService
    {
        public async Task ChangeUiTheme(ChangeUiThemeInput input)
        {
            await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), AppSettingNames.UiTheme, input.Theme);
        }
    }
}
