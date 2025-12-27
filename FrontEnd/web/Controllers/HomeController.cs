using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoppingMicroservices.FrontEnd.Web.Models.Dto;
using ShoppingMicroservices.FrontEnd.Web.Service.IService;
using web.Models;

namespace web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IProductService _productService;
    private readonly ICartService _cartService;

    public HomeController(
        ILogger<HomeController> logger,
        IProductService productService,
        ICartService cartService)
    {
        _logger = logger;
        this._productService = productService;
        this._cartService = cartService;
    }

    public async Task<IActionResult> Index()
    {
        ResponseDto? response = await _productService.GetAllProductsAsync();

        List<ProductDto> products = new();
        if (response != null && response.isSuccess)
        {
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            products = JsonSerializer.Deserialize<List<ProductDto>>(Convert.ToString(response!.Data!)!, options)!;
        }
        else
        {
            TempData["error"] = response?.Message;
        }
        return View(products);
    }

    [Authorize]
    public async Task<IActionResult> ProductDetails(int productId)
    {
        ResponseDto? response = await _productService.GetProductByIdAsync(productId);

        ProductDto productDto = new();
        if (response != null && response.isSuccess)
        {
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            productDto = JsonSerializer.Deserialize<ProductDto>(Convert.ToString(response!.Data!)!, options)!;
        }
        else
        {
            TempData["error"] = response?.Message;
        }
        return View(productDto);
    }
    [Authorize]
    [HttpPost]
    [ActionName("ProductDetails")]
    public async Task<IActionResult> ProductDetails(ProductDto productDto)
    {
        CartDto cartDto = new CartDto
        {
            CartHeaderDto = new CartHeaderDto
            {
                UserId = User.Claims.Where(c => c.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value,
            },

        };

        var cartDetails = new CartDetailsDto
        {
            Count = productDto.Count,
            ProductId = productDto.ProductId
        };
        List<CartDetailsDto> cartDetailsDtos = [cartDetails];
        cartDto.CartDetails = cartDetailsDtos;


        ResponseDto? response = await _cartService.UpsertCartAsync(cartDto);



        // ProductDto productDto = new();
        if (response != null && response.isSuccess)
        {
            TempData["success"] = "Item has been added to shopping cart";
            return RedirectToAction(nameof(Index));
        }
        else
        {
            TempData["error"] = response?.Message;
        }
        return View(productDto);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
