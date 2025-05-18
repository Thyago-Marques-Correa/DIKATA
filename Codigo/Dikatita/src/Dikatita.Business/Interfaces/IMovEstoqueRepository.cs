using Dikatita.Business.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dikatita.Business.Interfaces;

public interface IMovEstoqueRepository : IRepository<MovEstoque>
{
    Task<IEnumerable<MovEstoque>> ObterMovimentacoesPorProduto(Guid produtoId);
} 