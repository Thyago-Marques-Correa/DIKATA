using Dikatita.Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dikatita.Data.Mappings;

public class MovEstoqueMapping : IEntityTypeConfiguration<MovEstoque>
{
    public void Configure(EntityTypeBuilder<MovEstoque> builder)
    {
        builder.ToTable("MovEstoque");

        builder.HasKey(m => m.Id);

        builder.Property(m => m.TipoMovimentacao)
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(m => m.Quantidade)
            .IsRequired();

        builder.Property(m => m.Observacao)
            .HasMaxLength(500);

        builder.Property(m => m.DataMovimentacao)
            .IsRequired();

        builder.HasOne(m => m.Produto)
            .WithMany()
            .HasForeignKey(m => m.ProdutoId);
    }
} 