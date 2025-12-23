using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShoppingMicroservices.FrontEnd.Web.Models.Dto;
using ShoppingMicroservices.FrontEnd.Web.Models.Dtos;
using ShoppingMicroservices.FrontEnd.Web.Service.IService;
using ShoppingMicroservices.FrontEnd.Web.Utility;

namespace ShoppingMicroservices.FrontEnd.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly ITokenProvider _tokenProvider;
        private readonly ILogger<CouponController> _logger;

        public AuthController(
                    IAuthService authService,
                    ITokenProvider tokenProvider,
                    ILogger<CouponController> logger
                    )
        {
            this._authService = authService;
            this._tokenProvider = tokenProvider;
            this._logger = logger;
        }

        [HttpGet]
        public IActionResult Login()
        {
            LoginRequestDto loginRequestDto = new LoginRequestDto();
            return View(loginRequestDto);
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDto loginRequestDto)
        {
            ResponseDto? responseDto = await _authService.LoginAsync(loginRequestDto);

            if (responseDto != null && responseDto.isSuccess)
            {
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

                LoginResponseDto loginResponseDto = JsonSerializer.Deserialize<LoginResponseDto>(responseDto!.Data!.ToString()!, options)!;
                await SignInUser(loginResponseDto);
                _tokenProvider.SetToken(loginResponseDto.Token);
                return RedirectToAction("Index", "Home");
                // TempData["error"] = $"Registeration Faild: {responseDto.Message}";
                // return View(responseDto);

            }
            else
            {
                TempData["error"] = $"Login Faild: {responseDto.Message}";
                return View(loginRequestDto);
            }

            // else
            // {
            //     ModelState.AddModelError("CustomError", responseDto!.Message!);
            // }

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
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterationRequestDto registerationRequestDto)
        {
            ResponseDto? responseDto = await _authService.RegisterAsync(registerationRequestDto);

            ResponseDto? assignRoleResponse;
            if (responseDto != null && responseDto.isSuccess)
            {
                if (string.IsNullOrEmpty(registerationRequestDto.RoleName))
                {
                    registerationRequestDto.RoleName = SD.RoleCustomer;
                }

                assignRoleResponse = await _authService.AssignRoleAsync(registerationRequestDto);
                if (assignRoleResponse != null && assignRoleResponse.isSuccess)
                {
                    TempData["success"] = "Registeration Successful";
                    registerationRequestDto = new();
                    return RedirectToAction(nameof(Login));
                }
                else
                {
                    TempData["error"] = $"Registeration Faild: {responseDto.Message}";
                    return View(registerationRequestDto);
                }
            }
            else
            {
                TempData["error"] = $"Registeration Faild: {responseDto.Message}";
            }


            var roleList = new List<SelectListItem>()
            {
              new SelectListItem{Text=SD.RoleAdmin, Value=SD.RoleAdmin},
              new SelectListItem{Text=SD.RoleCustomer, Value=SD.RoleCustomer}
            };
            ViewBag.RoleList = roleList;

            return View(registerationRequestDto);
        }
        private async Task SignInUser(LoginResponseDto loginResponseDto)
        {

            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(loginResponseDto.Token);
            var identiry = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            // identiry.AddClaim(
            //     new Claim(JwtRegisteredClaimNames.Email, jwt.Claims.FirstOrDefault(c => c.Value == JwtRegisteredClaimNames.Email).Value)
            //     );

            identiry.AddClaims(new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Email, jwt.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Email)!.Value),
                new Claim(JwtRegisteredClaimNames.Sub, jwt.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)!.Value),
                new Claim(JwtRegisteredClaimNames.Name, jwt.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Name)!.Value),
                new Claim(ClaimTypes.Name, jwt.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Email)!.Value),
                new Claim(ClaimTypes.Role, jwt.Claims.FirstOrDefault(c => c.Type == "role")!.Value),
            });
            var principal = new ClaimsPrincipal(identiry);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

        }
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            _tokenProvider.ClearToken();
            return RedirectToAction("Index", "Home");
        }
    }
}