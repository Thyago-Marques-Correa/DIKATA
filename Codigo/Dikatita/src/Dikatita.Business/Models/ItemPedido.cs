namespace Dikatita.Business.Models;

public class ItemPedido : Entity
{
    public string NomeProduto { get; set; }
    public decimal ValorUnitario { get; set; }
    public int Quantidade { get; set; }

    public Guid ProdutoId { get; set; }
    public Produto Produto { get; set; }
    
    public Guid PedidoId { get; set; }
    public Pedido Pedido { get; set; }
}