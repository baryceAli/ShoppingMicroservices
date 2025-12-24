using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoppingMicroservices.Services.CouponAPI.Mapper;
using ShoppingMicroservices.Services.ProductAPI.Data;
using ShoppingMicroservices.Services.ProductAPI.Models.Dto;

namespace ShoppingMicroservices.Services.ProductAPI.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _repository;
        private readonly ResponseDto _response;

        public ProductController(IProductRepository repository)
        {
            this._repository = repository;
            _response = new ResponseDto();
        }


        [HttpGet]
        // [Authorize]
        public ActionResult<ResponseDto> Get()
        {
            try
            {
                var products = _repository.GetProducts();
                // var mapper = CouponMapper();
                _response.Data = ProductMapper.MapProductToDto(products);

                return Ok(_response);

            }
            catch (System.Exception ex)
            {
                _response.isSuccess = false;
                _response.Message = ex.Message;
                return BadRequest(_response);
            }
        }

        // [HttpGet("{id:int}")]
        [HttpGet("{id}", Name = "GetById")]
        // [Route("GetCouponById/{id:int}")]
        public ActionResult<ResponseDto> Get(int id)
        {
            try
            {
                var product = _repository.GetProductById(id);
                if (product == null)
                {
                    _response.isSuccess = false;
                    _response.Message = $"Couldn't find a product with Id: {id}";
                    return NotFound(_response);
                }
                _response.Data = ProductMapper.MapProductToDto(product);
                return Ok(_response);

            }
            catch (System.Exception ex)
            {
                _response.isSuccess = false;
                _response.Message = ex.Message;
                return BadRequest(_response);
            }
        }

        // [HttpGet("GetCouponByCode/{code}")]
        // public ActionResult<ResponseDto> GetCouponByCode(string code)
        // {
        //     try
        //     {
        //         var coupon = _repository.GetCouponByCode(code);
        //         if (coupon == null)
        //         {
        //             _response.isSuccess = false;
        //             _response.Message = $"Couldn't find coupon with code: {code}";

        //             return NotFound(_response);
        //         }
        //         _response.Data = CouponMapper.MapCouponToDto(coupon);
        //         return Ok(_response);
        //         // return Ok(coupon);

        //     }
        //     catch (System.Exception ex)
        //     {
        //         _response.isSuccess = false;
        //         _response.Message = ex.Message;

        //         return BadRequest(_response);
        //     }
        // }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Post([FromBody] ProductDto productDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _response.isSuccess = false;
                    _response.Message = $"Invalid Product";
                    return BadRequest(_response);
                }

                var createdProduct = _repository.AddProduct(ProductMapper.MapDtoToProduct(productDto));
                _response.Data = createdProduct;
                // return Created("GetCouponById", _response);
                return CreatedAtRoute(
                    "GetById",         // must match the Name of GET route
                    new { id = createdProduct.ProductId },  // route values
                    _response            // body
                );
            }
            catch (System.Exception ex)
            {
                _response.isSuccess = false;
                _response.Message = ex.Message;

                return BadRequest(_response);
            }
        }
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public ActionResult Put(int id, [FromBody] ProductDto productDto)
        {
            try
            {
                var product = ProductMapper.MapDtoToProduct(productDto);
                product.ProductId = id;
                _repository.UpdateProduct(product);
                _response.Message = "Updated Successfully";
                return Ok(_response);
            }
            catch (System.Exception ex)
            {
                _response.isSuccess = false;
                _response.Message = ex.Message;
                return BadRequest(_response);
            }
        }
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            try
            {
                _repository.DeleteProduct(id);
                _response.Message = "Deleted Successfully";
                return Ok(_response);
            }
            catch (System.Exception ex)
            {
                _response.isSuccess = false;
                _response.Message = ex.Message;
                return BadRequest(_response);
            }
        }

    }

}