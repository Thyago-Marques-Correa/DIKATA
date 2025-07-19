using Dikatita.Business.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dikatita.Business.Interfaces;

public interface IProdutoRepository : IRepository<Produto>
{
    Task AtualizarEstoque(Guid id, int quantidade, string tipoMovimentacao);
    Task<IEnumerable<Produto>> BuscarPorNome(string nome);
    Task<IEnumerable<Produto>> ObterPorIds(IEnumerable<Guid> ids);
}