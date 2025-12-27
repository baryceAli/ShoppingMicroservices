using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoppingMicroservices.FrontEnd.Web.Models.Dto;
using ShoppingMicroservices.FrontEnd.Web.Service.IService;

namespace ShoppingMicroservices.FrontEnd.Web.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            this._cartService = cartService;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            CartDto cartDto = await LoadCartDtoBasedOnLoggedInUser();
            return View(cartDto);
        }
        public async Task<IActionResult> Remove(int cartdetailsId)
        {
            var userId = User.Claims.Where(c => c.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
            var response = await _cartService.RemoveFromCartAsync(cartdetailsId);
            if (response != null && response.isSuccess)
            {
                TempData["success"] = "Cart updated successfully";
                return RedirectToAction(nameof(Index));
            }
            TempData["error"] = "Couldn't update the cart";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ApplyCoupon(CartDto cartDto)
        {
            cartDto.CartDetails = [];
            var response = await _cartService.ApplyCouponAsync(cartDto);
            if (response != null && response.isSuccess)
            {
                TempData["success"] = "Cart updated successfully";
                return RedirectToAction(nameof(Index));
            }
            TempData["error"] = "Couldn't update the cart";
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> RemoveCoupon(CartDto cartDto)
        {
            cartDto.CartDetails = [];
            cartDto.CartHeaderDto.CouponCode = "";
            var response = await _cartService.ApplyCouponAsync(cartDto);
            if (response != null && response.isSuccess)
            {
                TempData["success"] = "Cart updated successfully";
                return RedirectToAction(nameof(Index));
            }
            TempData["error"] = "Couldn't update the cart";
            return View();
        }


        private async Task<CartDto?> LoadCartDtoBasedOnLoggedInUser()
        {
            var userId = User.Claims.Where(c => c.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
            var response = await _cartService.GetCartByUserIdAsync(userId);
            if (response != null && response.isSuccess)
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };


                CartDto cartDto = JsonSerializer.Deserialize<CartDto>(response!.Data!.ToString()!, options)!;
                // var cartdetails= JsonSerializer.Deserialize<IEnumerable<CartDetailsDto>>()
                return cartDto;
            }
            return new CartDto();
        }
    }
}