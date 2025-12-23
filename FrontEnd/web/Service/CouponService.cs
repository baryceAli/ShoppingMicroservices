using ShoppingMicroservices.FrontEnd.Web.Models.Dtos;
using ShoppingMicroservices.FrontEnd.Web.Service.IService;
using ShoppingMicroservices.FrontEnd.Web.Utility;

namespace ShoppingMicroservices.FrontEnd.Web.Service
{
    public class CouponService : ICouponService
    {
        private readonly IBaseService _baseService;

        public CouponService(IBaseService baseService)
        {
            this._baseService = baseService;
        }
        public Task<ResponseDto?> CreateCouponAsync(AddCouponDto addCouponDto)
        {
            return _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.ApiType.POST,
                Data = addCouponDto,
                Url = $"{SD.CouponAPIBase}/api/coupon",

            });
        }

        public Task<ResponseDto?> DeleteCouponAsync(int id)
        {
            return _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.ApiType.DELETE,
                Url = $"{SD.CouponAPIBase}/api/coupon/{id}",
            });
        }

        public Task<ResponseDto?> GetAllCouponsAsync()
        {
            return _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.ApiType.GET,
                Url = $"{SD.CouponAPIBase}/api/coupon",
            });
        }

        public Task<ResponseDto?> GetCouponAsync(string code)
        {
            return _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.ApiType.GET,
                Url = $"{SD.CouponAPIBase}/api/coupon/GetCouponByCode/{code}",
            });
        }

        public Task<ResponseDto?> GetCouponByIdAsync(int id)
        {
            return _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.ApiType.GET,
                Url = $"{SD.CouponAPIBase}/api/coupon/{id}",
            });
        }

        public Task<ResponseDto?> UpdateCouponAsync(int id, AddCouponDto addCouponDto)
        {
            return _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.ApiType.PUT,
                Data = addCouponDto,
                Url = $"{SD.CouponAPIBase}/api/coupon/{id}",
            });
        }
    }

}