using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShoppingMicroservices.FrontEnd.Web.Models.Dto;
using ShoppingMicroservices.FrontEnd.Web.Service.IService;
using ShoppingMicroservices.FrontEnd.Web.Utility;

namespace ShoppingMicroservices.FrontEnd.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly ILogger<CouponController> _logger;

        public AuthController(
                    IAuthService authService,
                    ILogger<CouponController> logger
                    )
        {
            this._authService = authService;
            this._logger = logger;
        }

        [HttpGet]
        public IActionResult Login()
        {
            LoginRequestDto loginRequestDto = new LoginRequestDto();
            return View(loginRequestDto);
        }

        [HttpGet]
        public IActionResult Register()
        {
            var roleList = new List<SelectListItem>()
            {
              new SelectListItem{Text=SD.RoleAdmin, Value=SD.RoleAdmin},
              new SelectListItem{Text=SD.RoleCustomer, Value=SD.RoleCustomer}
            };
            ViewBag.RoleList = roleList;
            RegisterationRequestDto registerationRequestDto = new();
            return View(registerationRequestDto);
        }

        [HttpGet]
        public IActionResult Logout()
        {
            return View();
        }
    }
}