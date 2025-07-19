using Dikatita.Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dikatita.Data.Mappings;

public class ItemPedidoMapping : IEntityTypeConfiguration<ItemPedido>
{
    public void Configure(EntityTypeBuilder<ItemPedido> builder)
    {
        builder.ToTable("ItensPedido");
        
        builder.HasKey(x => x.Id);
        
        builder.Property(i => i.NomeProduto)
            .IsRequired()
            .HasMaxLength(150);
        
        builder.Property(i => i.ValorUnitario)
            .HasColumnType("decimal(18,2)")
            .IsRequired();
        
        builder.Property(i => i.Quantidade)
            .IsRequired();

        builder.HasOne(i => i.Produto)
            .WithMany()
            .HasForeignKey(i => i.ProdutoId);

        builder.HasOne(i => i.Pedido)
            .WithMany()
            .HasForeignKey(i => i.PedidoId);
    }
}