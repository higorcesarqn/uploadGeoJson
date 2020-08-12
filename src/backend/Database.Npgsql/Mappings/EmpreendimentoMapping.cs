using Domain.Entities;
using Infra.EntityFramework.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.Npgsql.Mappings
{
    public class EmpreendimentoMapping : IEntityTypeConfiguration<Empreendimento>
    {
        public void Configure(EntityTypeBuilder<Empreendimento> entity)
        {
            entity.ToTable("tb_empreendimento");

            entity.ConfigureEntityToNpsql("id");

            entity.HasKey(x => x.Id);
        
            entity
                .Property(x => x.Empreedimento)
                .IsRequired(false);

            entity
                .Property(x => x.Lote)
                .IsRequired(false);

            entity
                .Property(x => x.NumeroCadastro)
                .IsRequired(false);

            entity
                .Property(x => x.Area)
                .HasColumnName("area")
                .IsRequired(false);

            entity
                .Property(x => x.AreaDesapropriar)
                .IsRequired(false);

            entity
                .Property(x => x.NumeroProcesso)
                .IsRequired(false);

            entity
                .Property(x => x.Localizacao)
                .IsRequired(false);

            entity
                .HasIndex(x => x.Id)
                .HasName("index_id")
                .IsUnique();

            entity
               .HasIndex(x => x.Empreedimento)
               .IsUnique();

        }
    }
}
