using Domain.Entities;
using Infra.EntityFramework.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.Npgsql.Mappings
{
    public class GeojsonMapping : IEntityTypeConfiguration<Geojson>
    {
        public void Configure(EntityTypeBuilder<Geojson> entity)
        {
            entity.ToTable("tb_geojson");

            entity.ConfigureEntityToNpsql("id");

            entity.HasMany(x => x.Geometrias)
                .WithOne()
                .HasForeignKey("id_geojson")
                .OnDelete(DeleteBehavior.Cascade);
                
        }
    }
}
