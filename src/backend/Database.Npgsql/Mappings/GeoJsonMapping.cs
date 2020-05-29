using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.Npgsql.Mappings
{
    public class GeoJsonMapping : IEntityTypeConfiguration<GeoJson>
    {
        public void Configure(EntityTypeBuilder<GeoJson> entity)
        {
            entity.ToTable("tb_geo");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Properties)
                .HasColumnType("json");

        }
    }
}
