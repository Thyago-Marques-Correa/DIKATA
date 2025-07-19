namespace Dikatita.App.ViewModels;

public class ItemPedidoViewModel
{
    public Guid Id { get; set; }
    public string NomeProduto { get; set; }
    public decimal ValorUnitario { get; set; }
    public int Quantidade { get; set; }
}