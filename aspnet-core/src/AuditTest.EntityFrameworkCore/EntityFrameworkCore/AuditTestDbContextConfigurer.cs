using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace AuditTest.EntityFrameworkCore
{
    public static class AuditTestDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<AuditTestDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<AuditTestDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection);
        }
    }
}
