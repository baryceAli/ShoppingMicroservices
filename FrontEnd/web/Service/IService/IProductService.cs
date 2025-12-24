using ShoppingMicroservices.FrontEnd.Web.Models.Dto;

namespace ShoppingMicroservices.FrontEnd.Web.Service.IService
{
    public interface IProductService
    {
        // Task<ResponseDto?> GetProductAsync(string code);
        Task<ResponseDto?> GetAllProductsAsync();
        Task<ResponseDto?> GetProductByIdAsync(int id);
        Task<ResponseDto?> CreateProductAsync(ProductDto productDto);
        Task<ResponseDto?> UpdateProductAsync(int id, ProductDto productDto);
        Task<ResponseDto?> DeleteProductAsync(int id);

    }
}