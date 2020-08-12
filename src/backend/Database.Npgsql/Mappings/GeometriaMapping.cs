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
        
            entity
                .Property(x => x.Empreedimento)
                .HasColumnName("empreendimento")
                .IsRequired(false);

            entity
                .Property(x => x.Lote)
                .HasColumnName("lote")
                .IsRequired(false);

            entity
                .Property(x => x.NumeroCadastro)
                .HasColumnName("numero_cadastro")
                .IsRequired(false);

            entity
                .Property(x => x.Area)
                .HasColumnName("area")
                .IsRequired(false);

            entity
                .Property(x => x.AreaDesapropriar)
                .HasColumnName("area_desapropriar")
                .IsRequired(false);

            entity
                .Property(x => x.NumeroProcesso)
                .HasColumnName("numero_processo")
                .IsRequired(false);

            entity
                .Property(x => x.Localizacao)
                .HasColumnName("localizacao")
                .IsRequired(false);

            entity
                .HasIndex(x => x.Id)
                .HasName("index_id")
                .IsUnique();

            entity
               .HasIndex(x => x.Empreedimento)
               .HasName("index_empreendimento")
               .IsUnique();

        }
    }
}
