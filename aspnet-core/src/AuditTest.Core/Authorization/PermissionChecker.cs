using Abp.Authorization;
using AuditTest.Authorization.Roles;
using AuditTest.Authorization.Users;

namespace AuditTest.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {
        }
    }
}
