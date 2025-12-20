using Microsoft.AspNetCore.Mvc;
using ShoppingMicroservices.Data;
using ShoppingMicroservices.Data.Dto;
using ShoppingMicroservices.Services.CouponAPI.Data;
using ShoppingMicroservices.Services.CouponAPI.Mapper;
using ShoppingMicroservices.Services.CouponAPI.Models;
using ShoppingMicroservices.Services.CouponAPI.Models.Dtos;

namespace ShoppingMicroservices.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class CouponController : Controller
    {
        private readonly ICouponRepository _repository;
        private readonly ResponseDto _response;

        public CouponController(ICouponRepository repository)
        {
            this._repository = repository;
            _response = new ResponseDto();
        }

        [HttpGet]
        public ActionResult<IEnumerable<Coupon>> Get()
        {
            try
            {
                var coupons = _repository.GetCoupons();
                // var mapper = CouponMapper();

                return Ok(CouponMapper.MapCouponToDto(coupons));
                return Ok(coupons);

            }
            catch (System.Exception ex)
            {

                return BadRequest($"Error: {ex.Message}");
            }
        }

        // [HttpGet("{id:int}")]
        [HttpGet("{id}", Name = "GetCouponById")]
        // [Route("GetCouponById/{id:int}")]
        public ActionResult<ResponseDto> Get(int id)
        {
            try
            {
                var coupon = _repository.GetCouponById(id);
                if (coupon == null)
                {

                    return NotFound($"Couldn't find coupon with Id: {id}");
                }
                return Ok(CouponMapper.MapCouponToDto(coupon));
                return Ok(coupon);

            }
            catch (System.Exception ex)
            {

                return BadRequest($"Error: {ex.Message}");
            }
        }
        // [HttpGet("{id:int}")]
        [HttpGet("GetCouponByCode/{code}")]
        // [Route("GetCouponById/{id:int}")]
        public ActionResult<ResponseDto> GetCouponByCode(string code)
        {
            try
            {
                var coupon = _repository.GetCouponByCode(code);
                if (coupon == null)
                {

                    return NotFound($"Couldn't find coupon with code: {code}");
                }
                return Ok(CouponMapper.MapCouponToDto(coupon));
                // return Ok(coupon);

            }
            catch (System.Exception ex)
            {

                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpPost]
        public ActionResult Post([FromBody] AddCouponDto addCouponDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest($"Invalid Coupon");
                }
                var createdCoupon = _repository.AddCoupon(CouponMapper.MapAddCouponDtoToCoupon(addCouponDto));
                return CreatedAtRoute(
                    "GetCouponById",         // must match the Name of GET route
                    new { id = createdCoupon.CouponId },  // route values
                    createdCoupon            // body
                );
            }
            catch (System.Exception ex)
            {

                return BadRequest("Couldn't add coupon");
            }
        }
        [HttpPut("{id:int}")]
        public ActionResult Put(int id, [FromBody] Coupon coupon)
        {
            try
            {
                _repository.UpdateCoupon(coupon);
                return Ok();
            }
            catch (System.Exception ex)
            {
                return BadRequest($"Couldn't update coupon: {ex.Message}");
            }
        }
        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            try
            {
                _repository.DeleteCoupon(id);
                return Ok();
            }
            catch (System.Exception ex)
            {
                return BadRequest($"Couldn't Delete Coupon: {ex.Message}");
            }
        }

    }
}