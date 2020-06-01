using Domain.Entities;
using Infra.EntityFramework.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.Npgsql.Mappings
{
    public class GeometriaMapping : IEntityTypeConfiguration<Geometria>
    {
        public void Configure(EntityTypeBuilder<Geometria> entity)
        {
            entity.ToTable("tb_geometria");

            entity.ConfigureEntityToNpsql("id");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Properties)
                .HasColumnType("json");

            entity
                .HasIndex(x => x.Id)
                .HasName("index_id")
                .IsUnique();
        }
    }
}
