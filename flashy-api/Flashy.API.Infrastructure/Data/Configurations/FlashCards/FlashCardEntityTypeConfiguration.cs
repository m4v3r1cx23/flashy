using Flashy.API.Domain.Models.FlashCards;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Flashy.API.Infrastructure.Data.Configurations.FlashCards;

public class FlashCardEntityTypeConfiguration : IEntityTypeConfiguration<FlashCard>
{
  public void Configure(EntityTypeBuilder<FlashCard> builder)
  {
    builder
      .ToTable("FlashCards");

    builder
      .Property(p => p.Id)
      .HasColumnName(nameof(FlashCard.Id))
      .HasColumnType("uniqueidentifier")
      .ValueGeneratedOnAdd()
      .IsRequired();

    builder
      .Property(p => p.Question)
      .HasColumnName(nameof(FlashCard.Question))
      .HasColumnType("nvarchar(256)")
      .IsRequired()
      .IsUnicode();

    builder
      .Property(p => p.Answer)
      .HasColumnName(nameof(FlashCard.Answer))
      .HasColumnType("nvarchar(256)")
      .IsRequired()
      .IsUnicode();

    builder
      .Property(p => p.Hint)
      .HasColumnName(nameof(FlashCard.Hint))
      .HasColumnType("nvarchar(256)")
      .IsRequired()
      .IsUnicode();

    builder
      .Property(p => p.CreatedAt)
      .HasColumnName(nameof(FlashCard.CreatedAt))
      .HasColumnType("datetime2")
      .IsRequired();

    builder
      .Property(p => p.UpdatedAt)
      .HasColumnName(nameof(FlashCard.UpdatedAt))
      .HasColumnType("datetime2")
      .IsRequired();

    builder
      .Property(p => p.Hash)
      .HasColumnName(nameof(FlashCard.Hash))
      .HasColumnType("char(256)")
      .IsRequired();

    builder
      .Property("CreatedById")
      .HasColumnName("CreatedById")
      .HasColumnType("uniqueidentifier");

    builder
      .HasKey(k => k.Id)
      .HasName($"PK_{nameof(FlashCard)}");

    builder
      .HasIndex(i => i.Hash)
      .HasDatabaseName($"IX_{nameof(FlashCard)}_{nameof(FlashCard.Hash)}");

    builder.HasOne(r => r.CreatedBy)
      .WithMany()
      .HasForeignKey("CreatedById")
      .HasConstraintName($"FX_{nameof(FlashCard)}_{nameof(User)}_{nameof(User.Id)}")
      .OnDelete(DeleteBehavior.SetNull);
  }
}
