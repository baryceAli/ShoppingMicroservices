using System.Diagnostics;
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

    public HomeController(ILogger<HomeController> logger, IProductService productService)
    {
        _logger = logger;
        this._productService = productService;
    }

    public async Task<IActionResult> Index()
    {
        ResponseDto? response = await _productService.GetAllProductsAsync();

        List<ProductDto> products = new();
        if (response != null && response.isSuccess)
        {
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            products = JsonSerializer.Deserialize<List<ProductDto>>(Convert.ToString(response!.Data!), options);
        }
        else
        {
            TempData["error"] = response?.Message;
        }
        return View(products);
    }

    [Authorize]
    public async Task<IActionResult> Details(int productId)
    {
        ResponseDto? response = await _productService.GetProductByIdAsync(productId);

        ProductDto productDto = new();
        if (response != null && response.isSuccess)
        {
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            productDto = JsonSerializer.Deserialize<ProductDto>(Convert.ToString(response!.Data!), options);
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
