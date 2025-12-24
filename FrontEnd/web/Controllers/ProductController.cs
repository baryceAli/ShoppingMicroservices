using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using ShoppingMicroservices.FrontEnd.Web.Models.Dto;
using ShoppingMicroservices.FrontEnd.Web.Service.IService;

namespace ShoppingMicroservices.FrontEnd.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IProductService _productService;

        public ProductController(ILogger<ProductController> logger, IProductService productService)
        {
            this._productService = productService;
            this._logger = logger;
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

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductDto productDto)
        {
            //  response = new();
            if (ModelState.IsValid)
            {
                ResponseDto? response = await _productService.CreateProductAsync(productDto);
                if (response != null && response.isSuccess)
                {
                    TempData["success"] = "Product created successfully";
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
            return View(productDto);
        }

        public async Task<IActionResult> Edit(int productId)
        {
            ResponseDto? response = await _productService.GetProductByIdAsync(productId);
            // CouponDto couponDto = new CouponDto();
            if (response != null && response.isSuccess)
            {
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var productDto = JsonSerializer.Deserialize<ProductDto>(Convert.ToString(response!.Data!), options);
                return View(productDto);
            }

            return NotFound();
            // return View();
        }
        [HttpPost]
        public async Task<IActionResult> Edit(ProductDto productDto)
        {
            //  response = new();
            if (ModelState.IsValid)
            {
                ResponseDto? response = await _productService.UpdateProductAsync(productDto.ProductId, productDto);
                if (response != null && response.isSuccess)
                {
                    TempData["success"] = "Product Updated successfully";
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
            return View(productDto);
        }

        public async Task<IActionResult> Delete(int productId)
        {
            ResponseDto? response = await _productService.GetProductByIdAsync(productId);
            // CouponDto couponDto = new CouponDto();
            if (response != null && response.isSuccess)
            {
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var productDto = JsonSerializer.Deserialize<ProductDto>(Convert.ToString(response!.Data!), options);
                return View(productDto);
            }

            return NotFound();
            // return View();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(ProductDto productDto)
        {
            ResponseDto? response = await _productService.DeleteProductAsync(productDto.ProductId);
            // CouponDto couponDto = new CouponDto();
            if (response != null && response.isSuccess)
            {
                // var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                // var couponDto = JsonSerializer.Deserialize<CouponDto>(Convert.ToString(response!.Data!), options);
                TempData["success"] = "Product deleted successfully";
                return RedirectToAction(nameof(Index));

            }
            else
            {
                TempData["error"] = response?.Message;
            }


            return View(productDto);
        }

    }
}