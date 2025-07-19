using Dikatita.Business.Interfaces;
using Dikatita.Business.Models;
using Dikatita.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Dikatita.Data.Repository;

public class PedidoRepository : Repository<Pedido>, IPedidoRepository
{
    public PedidoRepository(DikatitaDbContext context) : base(context) { }

    public async Task<Pedido> ObterPedidoComItensPeloId(Guid pedidoId)
    {
        var result = await DbSet.AsNoTracking()
            .Include(p => p.Itens)
            .FirstOrDefaultAsync(p => p.Id == pedidoId);
        
        return result;
    }

    public async Task<IEnumerable<Pedido>> ObterPedidoComItensPeloCpf(string cpf)
    {
        var result = await DbSet.AsNoTracking()
            .Include(p => p.Itens)
            .Where(p => p.CpfCliente == cpf)
            .ToListAsync();
        
        return result;
    }
}