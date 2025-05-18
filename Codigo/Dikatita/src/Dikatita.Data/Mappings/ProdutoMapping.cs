using Dikatita.Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dikatita.Data.Mappings;

public class ProdutoMapping : IEntityTypeConfiguration<Produto>
{
    public void Configure(EntityTypeBuilder<Produto> builder)
    {
        builder.ToTable("Produtos");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Nome)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(p => p.Descricao)
            .HasMaxLength(500);

        builder.Property(p => p.Imagem)
            .HasMaxLength(250);

        builder.Property(p => p.Valor)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(p => p.DataCadastro)
            .IsRequired();

        builder.Property(p => p.Ativo)
            .IsRequired();
    }
}