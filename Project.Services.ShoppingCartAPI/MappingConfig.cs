using AutoMapper;
using Project.Services.ShoppingCartAPI.Models;
using Project.Services.ShoppingCartAPI.Models.Dto;

namespace Project.Services.ShoppingCartAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<CartHeader, CartHeaderDto>().ReverseMap();
                config.CreateMap<CartDetail, CartDetailsDto>().ReverseMap();
            });
            return mappingConfig;
        }
    }
}
