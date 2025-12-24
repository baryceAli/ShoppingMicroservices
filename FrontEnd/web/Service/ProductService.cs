using ShoppingMicroservices.FrontEnd.Web.Models.Dto;
using ShoppingMicroservices.FrontEnd.Web.Service.IService;
using ShoppingMicroservices.FrontEnd.Web.Utility;

namespace ShoppingMicroservices.FrontEnd.Web.Service
{
    public class ProductService : IProductService
    {
        private readonly IBaseService _baseService;

        public ProductService(IBaseService baseService)
        {
            this._baseService = baseService;
        }
        public async Task<ResponseDto?> CreateProductAsync(ProductDto productDto)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.ApiType.POST,
                Data = productDto,
                Url = $"{SD.ProductAPIBase}/api/Product"

            }, withBearer: true);
        }

        public async Task<ResponseDto?> DeleteProductAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.ApiType.DELETE,
                // Data = productDto,
                Url = $"{SD.ProductAPIBase}/api/Product/{id}"

            }, withBearer: true);
        }

        public async Task<ResponseDto?> GetAllProductsAsync()
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.ApiType.GET,
                // Data = productDto,
                Url = $"{SD.ProductAPIBase}/api/Product"

            }, withBearer: true);
        }

        // public async Task<ResponseDto?> GetProductAsync(string code)
        // {
        //     return await _baseService.SendAsync(new RequestDto
        //     {
        //         ApiType = SD.ApiType.GET,
        //         // Data = productDto,
        //         Url = $"{SD.ProductAPIBase}/api/Product/GetById/{code}"

        //     }, withBearer: true);
        // }

        public async Task<ResponseDto?> GetProductByIdAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.ApiType.GET,
                // Data = productDto,
                Url = $"{SD.ProductAPIBase}/api/Product/{id}"

            }, withBearer: true);
        }

        public async Task<ResponseDto?> UpdateProductAsync(int id, ProductDto productDto)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.ApiType.PUT,
                Data = productDto,
                Url = $"{SD.ProductAPIBase}/api/Product/{id}"

            }, withBearer: true);
        }
    }

}