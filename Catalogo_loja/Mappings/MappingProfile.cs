using AutoMapper;
using Catalogo_loja.Models;
using Catalogo_loja.DTOs;

namespace Catalogo_loja.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Produto, ProdutoDto>().ReverseMap();
    }
}