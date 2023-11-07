using Project.Services.OrderAPI.Models.Dto;

namespace Project.Services.OrderAPI.Service.IService
{
    public interface IProductSerivce
    {
        Task<IEnumerable<ProductDto>> GetProducts();

    }
}
