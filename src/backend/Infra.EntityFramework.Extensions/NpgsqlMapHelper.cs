using Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.EntityFramework.Extensions
{
    public static class NpgsqlMapHelper
    {
        public static EntityTypeBuilder<TEntity> ConfigureCreateAtAndUpdteAt<TEntity>(this EntityTypeBuilder<TEntity> entity) where TEntity : Entity
        {
            entity.Property(e => e.CreatedAt)
                .HasColumnName("data_inclusao")
                .HasDefaultValueSql("Now()");

            entity.Property(e => e.UpdatedAt)
                .IsRequired(false)
                .HasColumnName("data_atualizacao");

            return entity;
        }

        public static EntityTypeBuilder<TEntity> IgnoreInclusao<TEntity>(this EntityTypeBuilder<TEntity> entity) where TEntity : Entity
        {
            entity.Ignore(e => e.CreatedAt);
            entity.Ignore(e => e.UpdatedAt);
            return entity;
        }

        public static EntityTypeBuilder<TEntity> ConfigureKey<TEntity>(this EntityTypeBuilder<TEntity> entity, string columnsName) where TEntity : Entity
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id)
                .HasDefaultValueSql("gen_random_uuid()");

            entity.Property(e => e.Id)
               .HasColumnName(columnsName);

            return entity;
        }

        public static EntityTypeBuilder<TEntity> ConfigureEntityToNpsql<TEntity>(this EntityTypeBuilder<TEntity> entity, string keyName) where TEntity : Entity
        {
            entity.Property<long>("row")
           .UseSerialColumn();

            entity.HasIndex("row")
                    .IsUnique();

            ConfigureKey(entity, keyName);

            ConfigureCreateAtAndUpdteAt(entity);

            return entity;
        }
    }
}
