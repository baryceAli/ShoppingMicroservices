using ShoppingMicroservices.FrontEnd.Web.Models.Dto;
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
        public async Task<ResponseDto?> CreateCouponAsync(AddCouponDto addCouponDto)
        {



            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.ApiType.POST,
                Data = addCouponDto,
                Url = $"{SD.CouponAPIBase}/api/Coupon",
            }, withBearer: true);
        }

        public async Task<ResponseDto?> DeleteCouponAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.ApiType.DELETE,
                Url = $"{SD.CouponAPIBase}/api/coupon/{id}",
            }, withBearer: true);
        }

        public async Task<ResponseDto?> GetAllCouponsAsync()
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.ApiType.GET,
                Url = $"{SD.CouponAPIBase}/api/coupon",
            }, withBearer: true);
        }

        public async Task<ResponseDto?> GetCouponAsync(string code)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.ApiType.GET,
                Url = $"{SD.CouponAPIBase}/api/coupon/GetCouponByCode/{code}",
            }, withBearer: true);
        }

        public async Task<ResponseDto?> GetCouponByIdAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.ApiType.GET,
                Url = $"{SD.CouponAPIBase}/api/coupon/{id}",
            });
        }

        public async Task<ResponseDto?> UpdateCouponAsync(int id, AddCouponDto addCouponDto)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.ApiType.PUT,
                Data = addCouponDto,
                Url = $"{SD.CouponAPIBase}/api/coupon/{id}",
            });
        }
    }

}