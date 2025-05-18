using AutoMapper;
using Dikatita.App.Models;
using Dikatita.Business.Models;

namespace Dikatita.App.AutoMapper;

public class AutoMapperConfig : Profile
{
    public AutoMapperConfig()
    {
        CreateMap<Produto, ProdutoViewModel>().ReverseMap();
        CreateMap<MovEstoque, MovEstoqueViewModel>().ReverseMap();
    }
}