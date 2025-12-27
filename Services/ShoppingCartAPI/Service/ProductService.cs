using System.Text.Json;
using ShoppingMicroservices.Services.ShoppingCartAPI.Models.Dto;
using ShoppingMicroservices.Services.ShoppingCartAPI.Service.IService;

namespace ShoppingMicroservices.Services.ShoppingCartAPI.Service
{
    public class ProductService : IProductService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ProductService(IHttpClientFactory httpClientFactory)
        {
            this._httpClientFactory = httpClientFactory;
        }
        public async Task<IEnumerable<ProductDto>> GetProducts()
        {
            try
            {
                var client = _httpClientFactory.CreateClient("Product");
                var response = await client.GetAsync("/api/product");
                var apiContent = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var resp = JsonSerializer.Deserialize<ResponseDto>(apiContent.ToString(), options);
                if (resp != null && resp.isSuccess)
                {
                    return JsonSerializer.Deserialize<IEnumerable<ProductDto>>(resp!.Data!.ToString()!, options)!;
                }
                // return new List<ProductDto>();
            }
            catch (System.Exception ex)
            {
                // throw;
            }
            return new List<ProductDto>();
        }
    }

}