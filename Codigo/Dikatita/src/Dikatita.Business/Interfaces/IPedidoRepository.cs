using Dikatita.Business.Models;

namespace Dikatita.Business.Interfaces;

public interface IPedidoRepository : IRepository<Pedido>
{
    public Task<Pedido> ObterPedidoComItensPeloId(Guid pedidoId);
    public Task<IEnumerable<Pedido>> ObterPedidoComItensPeloCpf(string cpf);
}