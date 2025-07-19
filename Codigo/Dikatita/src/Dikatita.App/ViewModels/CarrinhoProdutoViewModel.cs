namespace Dikatita.App.Models;

public class CarrinhoProdutoViewModel
{
    public Guid Id { get; set; }
    public ICollection<ProdutoViewModel> Produtos { get; set; } = new List<ProdutoViewModel>();
    public int QuantidadeCarrinho { get; set; }
}