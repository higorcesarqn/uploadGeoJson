using Database.Npgsql.Mappings;
using Infra.EntityFramework.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Database.Npgsql
{
    public class NpgsqlContext : DbContext
    {
        public NpgsqlContext(DbContextOptions<NpgsqlContext> dbContextOptions) : base(dbContextOptions)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("pgcrypto")
              .HasPostgresExtension("postgis")
              .HasPostgresExtension("tablefunc");

            modelBuilder.ApplyAllConfigurationsFromCurrentAssembly(typeof(GeoJsonMapping).Assembly);

            modelBuilder.ApplySnakeCaseInColumnName();

            base.OnModelCreating(modelBuilder);
        }
    }
}
