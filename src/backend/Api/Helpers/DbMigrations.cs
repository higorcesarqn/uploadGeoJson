using Database.Npgsql;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

namespace Api.Helpers
{
    public static class DbMigrations
    {
        //Add-Migration DbInit -context NpgsqlContext -output Data/Migrations
        public static async Task EnsureDatabasesMigrated(IHost host)
        {
            using var serviceScope = host.Services.CreateScope();
            var services = serviceScope.ServiceProvider;

            await EnsureDatabasesMigrated<NpgsqlContext>(services);
        }

        private static async Task EnsureDatabasesMigrated<TDbContext>(IServiceProvider services)
            where TDbContext : DbContext
        {
            using var scope = services.GetRequiredService<IServiceScopeFactory>().CreateScope();

            using var context = scope.ServiceProvider.GetRequiredService<TDbContext>();
            await context.Database.MigrateAsync();
        }
    }
}
