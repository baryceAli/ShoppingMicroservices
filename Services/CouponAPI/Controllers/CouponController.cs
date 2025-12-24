using Microsoft.AspNetCore.Mvc;
using ShoppingMicroservices.Data;

using ShoppingMicroservices.Services.CouponAPI.Mapper;
using ShoppingMicroservices.Services.CouponAPI.Models.Dtos;
using ShoppingMicroservices.Services.CouponAPI.Models.Dto;
using Microsoft.AspNetCore.Authorization;

namespace ShoppingMicroservices.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class CouponController : ControllerBase
    {
        private readonly ICouponRepository _repository;
        private readonly ResponseDto _response;

        public CouponController(ICouponRepository repository)
        {
            this._repository = repository;
            _response = new ResponseDto();
        }

        [HttpGet]
        [Authorize]
        public ActionResult<ResponseDto> Get()
        {
            try
            {
                var coupons = _repository.GetCoupons();
                // var mapper = CouponMapper();
                _response.Data = CouponMapper.MapCouponToDto(coupons);

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
        [HttpGet("{id}", Name = "GetCouponById")]
        // [Route("GetCouponById/{id:int}")]
        public ActionResult<ResponseDto> Get(int id)
        {
            try
            {
                var coupon = _repository.GetCouponById(id);
                if (coupon == null)
                {
                    _response.isSuccess = false;
                    _response.Message = $"Couldn't find coupon with Id: {id}";
                    return NotFound(_response);
                }
                _response.Data = CouponMapper.MapCouponToDto(coupon);
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
        [HttpGet("GetCouponByCode/{code}")]
        // [Route("GetCouponById/{id:int}")]
        public ActionResult<ResponseDto> GetCouponByCode(string code)
        {
            try
            {
                var coupon = _repository.GetCouponByCode(code);
                if (coupon == null)
                {
                    _response.isSuccess = false;
                    _response.Message = $"Couldn't find coupon with code: {code}";

                    return NotFound(_response);
                }
                _response.Data = CouponMapper.MapCouponToDto(coupon);
                return Ok(_response);
                // return Ok(coupon);

            }
            catch (System.Exception ex)
            {
                _response.isSuccess = false;
                _response.Message = ex.Message;

                return BadRequest(_response);
            }
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public ActionResult Post([FromBody] AddCouponDto addCouponDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _response.isSuccess = false;
                    _response.Message = $"Invalid Coupon";
                    return BadRequest(_response);
                }

                var createdCoupon = _repository.AddCoupon(CouponMapper.MapAddCouponDtoToCoupon(addCouponDto));
                _response.Data = createdCoupon;
                // return Created("GetCouponById", _response);
                return CreatedAtRoute(
                    "GetCouponById",         // must match the Name of GET route
                    new { id = createdCoupon.CouponId },  // route values
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
        [Authorize(Roles = "ADMIN")]
        public ActionResult Put(int id, [FromBody] AddCouponDto addCouponDto)
        {
            try
            {
                var coupon = CouponMapper.MapAddCouponDtoToCoupon(addCouponDto);
                _repository.UpdateCoupon(coupon);
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
        [Authorize(Roles = "ADMIN")]
        public ActionResult Delete(int id)
        {
            try
            {
                _repository.DeleteCoupon(id);
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