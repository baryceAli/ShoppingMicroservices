using ShoppingMicroservices.Services.ShoppingCartAPI.Models.Dto;

namespace ShoppingMicroservices.Services.ShoppingCartAPI.Service.IService
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetProducts();
    }
}