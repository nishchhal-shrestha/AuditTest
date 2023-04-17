using Abp.AspNetCore;
using Abp.AspNetCore.TestBase;
using Abp.Modules;
using Abp.Reflection.Extensions;
using AuditTest.EntityFrameworkCore;
using AuditTest.Web.Startup;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

namespace AuditTest.Web.Tests
{
    [DependsOn(
        typeof(AuditTestWebMvcModule),
        typeof(AbpAspNetCoreTestBaseModule)
    )]
    public class AuditTestWebTestModule : AbpModule
    {
        public AuditTestWebTestModule(AuditTestEntityFrameworkModule abpProjectNameEntityFrameworkModule)
        {
            abpProjectNameEntityFrameworkModule.SkipDbContextRegistration = true;
        } 
        
        public override void PreInitialize()
        {
            Configuration.UnitOfWork.IsTransactional = false; //EF Core InMemory DB does not support transactions.
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(AuditTestWebTestModule).GetAssembly());
        }
        
        public override void PostInitialize()
        {
            IocManager.Resolve<ApplicationPartManager>()
                .AddApplicationPartsIfNotAddedBefore(typeof(AuditTestWebMvcModule).Assembly);
        }
    }
}