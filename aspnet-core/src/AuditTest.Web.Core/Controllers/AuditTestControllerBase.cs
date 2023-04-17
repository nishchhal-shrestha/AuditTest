using Abp.AspNetCore.Mvc.Controllers;
using Abp.IdentityFramework;
using Microsoft.AspNetCore.Identity;

namespace AuditTest.Controllers
{
    public abstract class AuditTestControllerBase: AbpController
    {
        protected AuditTestControllerBase()
        {
            LocalizationSourceName = AuditTestConsts.LocalizationSourceName;
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
