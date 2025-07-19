using AutoMapper;
using Dikatita.App.Models;
using Dikatita.App.ViewModels;
using Dikatita.Business.Models;

namespace Dikatita.App.AutoMapper;

public class AutoMapperConfig : Profile
{
    public AutoMapperConfig()
    {
        CreateMap<Produto, ProdutoViewModel>().ReverseMap();
        CreateMap<MovEstoque, MovEstoqueViewModel>().ReverseMap();
        CreateMap<ItemPedido, ItemPedidoViewModel>().ReverseMap();
        CreateMap<Pedido, PedidoViewModel>().ReverseMap();
        CreateMap<Pedido, EdicaoPedidoViewModel>().ReverseMap();
    }
}