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
                var response = await client.GetAsync($"/api/coupon/GetByCode/{couponCode}");
                var apiContent = await response.Content.ReadAsStringAsync();
                var resp = JsonSerializer.Deserialize<ResponseDto>(apiContent.ToString());
                if (resp.isSuccess)
                {
                    return JsonSerializer.Deserialize<CouponDto>(resp!.Data!.ToString())!;
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