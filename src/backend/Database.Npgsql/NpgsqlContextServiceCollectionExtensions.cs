using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Database.Npgsql
{
    public static class NpgsqlContextServiceCollectionExtensions
    {
        public static IServiceCollection AddNpsqlDbContext(this IServiceCollection services, string connectionString)
        {
            services.AddEntityFrameworkNpgsql()
                .AddDbContext<NpgsqlContext>(
                options => options
                .UseNpgsql(
                    connectionString, o => { o.UseNetTopologySuite(); o.MigrationsAssembly("Api"); }));


            return services;
        }
    }
}
