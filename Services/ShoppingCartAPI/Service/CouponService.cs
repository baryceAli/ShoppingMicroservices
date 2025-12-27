using System.Text.Json;
using ShoppingMicroservices.Services.ShoppingCartAPI.Models.Dto;
using ShoppingMicroservices.Services.ShoppingCartAPI.Service.IService;

namespace ShoppingMicroservices.Services.ShoppingCartAPI.Service
{
    public class CouponService : ICouponService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CouponService(IHttpClientFactory httpClientFactory)
        {
            this._httpClientFactory = httpClientFactory;
        }
        public async Task<CouponDto> GetCoupon(string couponCode)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("Coupon");
                var response = await client.GetAsync($"/api/coupon/GetCouponByCode/{couponCode}");
                var apiContent = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var resp = JsonSerializer.Deserialize<ResponseDto>(apiContent.ToString(), options);
                if (resp.isSuccess)
                {

                    return JsonSerializer.Deserialize<CouponDto>(resp!.Data!.ToString()!, options)!;
                }
                // return new List<ProductDto>();
            }
            catch (System.Exception ex)
            {
                // throw;
            }
            return new CouponDto();

        }
    }

}