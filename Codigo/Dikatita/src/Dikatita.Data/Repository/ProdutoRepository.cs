using Dikatita.Business.Interfaces;
using Dikatita.Business.Models;
using Dikatita.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dikatita.Data.Repository;

public class ProdutoRepository : Repository<Produto>, IProdutoRepository
{
    public ProdutoRepository(DikatitaDbContext context) : base(context) { }

    public async Task AtualizarEstoque(Guid id, int quantidade, string tipoMovimentacao)
    {
        var produto = await ObterPorId(id);
        
        if (produto == null) return;

        if (tipoMovimentacao == "Entrada")
        {
            produto.QuantidadeEstoque += quantidade;
        }
        else if (tipoMovimentacao == "Saída")
        {
            // Verificar se há estoque suficiente
            if (produto.QuantidadeEstoque < quantidade)
            {
                throw new InvalidOperationException("Estoque insuficiente para realizar a saída");
            }
            
            produto.QuantidadeEstoque -= quantidade;
        }

        Context.Update(produto);
        await SaveChanges();
    }

    public async Task<IEnumerable<Produto>> BuscarPorNome(string nome)
    {
        return await DbSet.AsNoTracking()
            .Where(p => p.Nome.ToLower().Contains(nome.ToLower()) && p.Ativo && p.QuantidadeEstoque > 0)
            .ToListAsync();
    }
}