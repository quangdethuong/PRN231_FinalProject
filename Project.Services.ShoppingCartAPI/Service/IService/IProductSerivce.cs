﻿using Project.Services.ShoppingCartAPI.Models.Dto;

namespace Project.Services.ShoppingCartAPI.Service.IService
{
    public interface IProductSerivce
    {
        Task<IEnumerable<ProductDto>> GetProducts();

    }
}
