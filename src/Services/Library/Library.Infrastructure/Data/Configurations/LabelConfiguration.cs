using Library.Domain.Models;
using Library.Domain.ValueObjects.Ids;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Infrastructure.Data.Configurations;

public class LabelConfiguration : IEntityTypeConfiguration<Label>
{
    public void Configure(EntityTypeBuilder<Label> entity)
    {
        entity.ToTable("labels");

        entity.HasKey(e => e.Id);

        entity.Property(e => e.Id)
            .HasColumnName("id")
            .HasConversion(labelId => labelId.Value, dbId => LabelId.Of(dbId))
            .ValueGeneratedNever();

        entity.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(200)
            .HasColumnName("name");

        entity.HasIndex(e => e.Name)
            .IsUnique()
            .HasDatabaseName("ix_labels_name");
    }
}
