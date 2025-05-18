using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Dikatita.App.Models;

public class MovEstoqueViewModel
{
    [Key]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [DisplayName("Produto")]
    public Guid ProdutoId { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [DisplayName("Tipo de Movimentação")]
    public string TipoMovimentacao { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [Range(1, int.MaxValue, ErrorMessage = "A quantidade deve ser maior que 0")]
    public int Quantidade { get; set; }

    [DisplayName("Observação")]
    [StringLength(500, ErrorMessage = "O campo {0} precisa ter no máximo {1} caracteres")]
    public string Observacao { get; set; }

    [ScaffoldColumn(false)]
    public DateTime DataMovimentacao { get; set; } = DateTime.Now;

    public ProdutoViewModel Produto { get; set; }
    public IEnumerable<ProdutoViewModel> Produtos { get; set; }
} 