using System.Collections;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using ShoppingMicroservices.FrontEnd.Web.Models.Dtos;
using ShoppingMicroservices.FrontEnd.Web.Service;
using ShoppingMicroservices.FrontEnd.Web.Service.IService;

namespace ShoppingMicroservices.FrontEnd.Web.Controllers
{
    public class CouponController : Controller
    {
        private readonly ICouponService _couponService;
        private readonly ILogger<CouponController> _logger;

        public CouponController(
            ICouponService couponService,
            ILogger<CouponController> logger
            )
        {
            this._couponService = couponService;
            this._logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            ResponseDto? response = await _couponService.GetAllCouponsAsync();

            List<CouponDto> coupons = new();
            if (response != null && response.isSuccess)
            {
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                ;
                coupons = JsonSerializer.Deserialize<List<CouponDto>>(Convert.ToString(response!.Data!), options);
            }

            return View(coupons);
        }

    }
}