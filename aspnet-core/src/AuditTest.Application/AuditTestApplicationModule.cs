using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using AuditTest.Authorization;

namespace AuditTest
{
    [DependsOn(
        typeof(AuditTestCoreModule), 
        typeof(AbpAutoMapperModule))]
    public class AuditTestApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Authorization.Providers.Add<AuditTestAuthorizationProvider>();
        }

        public override void Initialize()
        {
            var thisAssembly = typeof(AuditTestApplicationModule).GetAssembly();

            IocManager.RegisterAssemblyByConvention(thisAssembly);

            Configuration.Modules.AbpAutoMapper().Configurators.Add(
                // Scan the assembly for classes which inherit from AutoMapper.Profile
                cfg => cfg.AddMaps(thisAssembly)
            );
        }
    }
}
