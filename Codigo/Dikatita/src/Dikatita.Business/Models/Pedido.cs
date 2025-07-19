using Dikatita.Business.Enums;

namespace Dikatita.Business.Models;

public class Pedido : Entity
{
    public DateTime DataCadastro { get; set; }
    public string NomeCliente { get; set; }
    public string TelefoneCliente { get; set; }
    public string CpfCliente { get; set; }
    public StatusPedido Status { get; set; } = StatusPedido.Ativo;
    public ICollection<ItemPedido> Itens { get; set; } = [];
}