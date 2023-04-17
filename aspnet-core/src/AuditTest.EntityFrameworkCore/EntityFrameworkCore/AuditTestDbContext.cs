using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using AuditTest.Authorization.Roles;
using AuditTest.Authorization.Users;
using AuditTest.MultiTenancy;
using AuditTest.Entity;

namespace AuditTest.EntityFrameworkCore
{
    public class AuditTestDbContext : AbpZeroDbContext<Tenant, Role, User, AuditTestDbContext>
    {
        /* Define a DbSet for each entity of the application */

        public DbSet<Department> Departments { get; set; }

        public AuditTestDbContext(DbContextOptions<AuditTestDbContext> options)
            : base(options)
        {
        }
    }
}
