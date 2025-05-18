using System;

namespace Dikatita.Business.Models;

public class MovEstoque : Entity
{
    public Guid ProdutoId { get; set; }
    public string TipoMovimentacao { get; set; }  // "Entrada" ou "Saída"
    public int Quantidade { get; set; }
    public string Observacao { get; set; }
    public DateTime DataMovimentacao { get; set; }
    
    /* EF Relations */
    public Produto Produto { get; set; }
} 