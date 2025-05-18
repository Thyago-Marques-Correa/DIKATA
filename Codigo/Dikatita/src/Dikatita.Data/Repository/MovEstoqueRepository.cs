using Dikatita.Business.Interfaces;
using Dikatita.Business.Models;
using Dikatita.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dikatita.Data.Repository;

public class MovEstoqueRepository : Repository<MovEstoque>, IMovEstoqueRepository
{
    public MovEstoqueRepository(DikatitaDbContext context) : base(context) { }

    public async Task<IEnumerable<MovEstoque>> ObterMovimentacoesPorProduto(Guid produtoId)
    {
        return await DbSet.AsNoTracking()
            .Where(m => m.ProdutoId == produtoId)
            .OrderByDescending(m => m.DataMovimentacao)
            .ToListAsync();
    }
} 