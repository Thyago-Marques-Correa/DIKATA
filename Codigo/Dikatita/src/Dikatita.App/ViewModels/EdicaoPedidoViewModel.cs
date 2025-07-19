using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Dikatita.Business.Enums;

namespace Dikatita.App.ViewModels;

public class EdicaoPedidoViewModel
{
    [Key]
    public Guid Id { get; set; }
    
    [DisplayName("Data Cadastro")]
    public DateTime DataCadastro { get; set; }
    
    [DisplayName("Nome do Cliente")]
    public string NomeCliente { get; set; }
    
    [DisplayName("Telefone")]
    public string TelefoneCliente { get; set; }
    
    [DisplayName("CPF")]     
    public string CpfCliente { get; set; }
    
    public StatusPedido Status { get; set; }
    public ICollection<ItemPedidoViewModel> Itens { get; set; } = [];
}