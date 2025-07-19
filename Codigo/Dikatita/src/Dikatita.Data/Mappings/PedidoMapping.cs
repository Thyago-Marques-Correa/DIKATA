using Dikatita.Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dikatita.Data.Mappings;

public class PedidoMapping : IEntityTypeConfiguration<Pedido>
{
    public void Configure(EntityTypeBuilder<Pedido> builder)
    {
        builder.ToTable("Pedidos");
        
        builder.HasKey(p => p.Id);
        
        builder.Property(p => p.DataCadastro)
            .IsRequired();
        
        builder.Property(p => p.NomeCliente)
            .HasColumnType("varchar(100)")
            .IsRequired();
        
        builder.Property(p => p.CpfCliente)
            .HasColumnType("varchar(15)")
            .IsRequired();
        
        builder.Property(p => p.TelefoneCliente)
            .HasColumnType("varchar(20)")
            .IsRequired();
        
        builder.Property(p => p.Status)
            .IsRequired();
        
        builder.HasMany(p => p.Itens)
            .WithOne(p => p.Pedido)
            .HasForeignKey(p => p.PedidoId);
    }
}