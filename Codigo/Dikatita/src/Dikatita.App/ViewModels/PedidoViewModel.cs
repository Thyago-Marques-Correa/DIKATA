using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Dikatita.Business.Enums;

namespace Dikatita.App.ViewModels;

public class PedidoViewModel
{
    [Key]
    public Guid Id { get; set; }
    
    [DisplayName("Data Cadastro")]
    public DateTime DataCadastro { get; set; }
    
    [DisplayName("Nome do Cliente")]
    [Required(ErrorMessage = "Digite o Nome do cliente")]
    public string NomeCliente { get; set; }
    
    [DisplayName("Telefone")]
    [Required(ErrorMessage = "Digite o Telefone do cliente")]
    public string TelefoneCliente { get; set; }
    
    [DisplayName("CPF")]     
    [Required(ErrorMessage = "Digite o CPF do cliente")]
    public string CpfCliente { get; set; }
    
    public StatusPedido Status { get; set; }
    public ICollection<ItemPedidoViewModel> Itens { get; set; } = [];
}