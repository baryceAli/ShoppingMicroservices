using System.Collections;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using ShoppingMicroservices.FrontEnd.Mapper;
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
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                coupons = JsonSerializer.Deserialize<List<CouponDto>>(Convert.ToString(response!.Data!), options);
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View(coupons);
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CouponDto couponDto)
        {
            //  response = new();
            if (ModelState.IsValid)
            {
                ResponseDto? response = await _couponService.CreateCouponAsync(CouponMapper.MapCouponDtoToAddCouponDto(couponDto));
                if (response != null && response.isSuccess)
                {
                    TempData["success"] = "Coupon created successfully";
                    return RedirectToAction(nameof(Index));

                }
                else
                {
                    TempData["error"] = response?.Message;
                }

            }
            else
            {
                TempData["error"] = "Please make sure you entered a valid data";
            }
            return View(couponDto);
        }

        public async Task<IActionResult> Delete(int id)
        {
            ResponseDto? response = await _couponService.GetCouponByIdAsync(id);
            // CouponDto couponDto = new CouponDto();
            if (response != null && response.isSuccess)
            {
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var couponDto = JsonSerializer.Deserialize<CouponDto>(Convert.ToString(response!.Data!), options);
                return View(couponDto);
            }

            return NotFound();
            // return View();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(CouponDto couponDto)
        {
            ResponseDto? response = await _couponService.DeleteCouponAsync(couponDto.CouponId);
            // CouponDto couponDto = new CouponDto();
            if (response != null && response.isSuccess)
            {
                // var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                // var couponDto = JsonSerializer.Deserialize<CouponDto>(Convert.ToString(response!.Data!), options);
                TempData["success"] = "Coupon deleted successfully";
                return RedirectToAction(nameof(Index));

            }
            else
            {
                TempData["error"] = response?.Message;
            }


            return View(couponDto);
        }

    }
}