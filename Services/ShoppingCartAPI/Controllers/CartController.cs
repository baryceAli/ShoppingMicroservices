using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingMicroservices.Services.ShoppingCartAPI.Data;
using ShoppingMicroservices.Services.ShoppingCartAPI.Mapper;
using ShoppingMicroservices.Services.ShoppingCartAPI.Models;
using ShoppingMicroservices.Services.ShoppingCartAPI.Models.Dto;
using ShoppingMicroservices.Services.ShoppingCartAPI.Service;
using ShoppingMicroservices.Services.ShoppingCartAPI.Service.IService;

namespace ShoppingMicroservices.Services.ShoppingCartAPI.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IProductService _productService;
        private readonly ICouponService _couponService;
        private readonly ResponseDto _response;
        public CartController(
            AppDbContext context,
            IProductService productService,
            ICouponService couponService)
        {
            this._context = context;
            this._productService = productService;
            this._couponService = couponService;
            _response = new();
        }

        [HttpGet("GetCart/{userId}")]
        public async Task<ResponseDto> GetCart(string userId)
        {
            Console.WriteLine("Inside GetCartAPI");
            var products = await _productService.GetProducts();
            try
            {
                CartHeader cartHeader = await _context.CartHeaders.FirstOrDefaultAsync(ch => ch.UserId == userId);
                if (cartHeader == null)
                {
                    _response.isSuccess = false;
                    _response.Data = $"No Carts found for user: {userId}";
                }
                IEnumerable<CartDetails> cartDetailsList = _context.CartDetails.Where(cd => cd.CartHeaderId == cartHeader.CartHeaderId);
                var examineCartDetails = cartDetailsList.ToList();
                CartDto cartDto = new CartDto
                {
                    CartHeaderDto = ShoppingCartMapper.MapCartHeaderToDto(cartHeader),
                    CartDetails = ShoppingCartMapper.MapCartDetailsToDto(cartDetailsList.ToList())
                };

                var ss = cartDto;
                List<CartDetailsDto> newCartDet = new();
                foreach (var item in cartDto.CartDetails)
                {
                    CartDetailsDto cd = new();
                    cd = item;

                    var prod = products.First(p => p.ProductId == item.ProductId);
                    cd.Product = prod;
                    cartDto.CartHeaderDto.CartTotal += item.Count * item.Product.Price;

                    newCartDet.Add(cd);
                }
                cartDto.CartDetails = newCartDet;

                // Apply Coupon
                if (!string.IsNullOrEmpty(cartDto.CartHeaderDto.CouponCode))
                {
                    var coupon = await _couponService.GetCoupon(cartDto.CartHeaderDto.CouponCode);
                    if (coupon != null && cartDto.CartHeaderDto.CartTotal > coupon.MinAmount)
                    {
                        cartDto.CartHeaderDto.CartTotal -= coupon.DiscountAmount;
                        cartDto.CartHeaderDto.Discount = coupon.DiscountAmount;

                    }
                }
                _response.Data = cartDto;
            }
            catch (System.Exception ex)
            {
                _response.isSuccess = false;
                _response.Data = ex.Message;
            }

            return _response;
        }

        [HttpPost("ApplyCoupon")]
        public async Task<ResponseDto> ApplyCoupon([FromBody] CartDto cartDto)
        {
            Console.WriteLine("--> CartAPI: ApplyCoupon");
            try
            {
                var cartHeaderFromDB = await _context.CartHeaders.FirstAsync(ch => ch.UserId == cartDto.CartHeaderDto.UserId);
                cartHeaderFromDB.CouponCode = cartDto.CartHeaderDto.CouponCode;
                _context.CartHeaders.Update(cartHeaderFromDB);
                await _context.SaveChangesAsync();
                _response.Data = true;
            }
            catch (System.Exception ex)
            {

                _response.isSuccess = false;
                _response.Data = ex.Message;
            }
            return _response;
        }

        [HttpPost("RemoveCoupon")]
        public async Task<ResponseDto> RemoveCoupon([FromBody] CartDto cartDto)
        {
            try
            {
                var cartHeaderFromDB = await _context.CartHeaders.FirstAsync(ch => ch.UserId == cartDto.CartHeaderDto.UserId);
                cartHeaderFromDB.CouponCode = "";
                _context.CartHeaders.Update(cartHeaderFromDB);
                await _context.SaveChangesAsync();
                _response.Data = true;
            }
            catch (System.Exception ex)
            {

                _response.isSuccess = false;
                _response.Data = ex.Message;
            }
            return _response;
        }

        [HttpPost("CartUpsert")]
        public async Task<ResponseDto> CartUpsert(CartDto cartDto)
        {
            try
            {
                var cartHeaderFromDB = await _context.CartHeaders.AsNoTracking().FirstOrDefaultAsync(u => u.UserId == cartDto.CartHeaderDto.UserId);
                if (cartHeaderFromDB == null)
                {
                    //create new CardHeader
                    var cartHeader = ShoppingCartMapper.MapDtoToCartHeader(cartDto.CartHeaderDto);
                    await _context.CartHeaders.AddAsync(cartHeader);
                    await _context.SaveChangesAsync();

                    cartDto.CartDetails.First().CartHeaderId = cartHeader.CartHeaderId;
                    await _context.CartDetails.AddAsync(ShoppingCartMapper.MapDtoToCartDetails(cartDto.CartDetails.First()));
                    await _context.SaveChangesAsync();

                }
                else
                {
                    var cartDetailsFromDB = await _context.CartDetails.AsNoTracking().FirstOrDefaultAsync(
                            u => u.ProductId == cartDto.CartDetails.First().ProductId &&
                            u.CartHeaderId == cartHeaderFromDB.CartHeaderId);
                    if (cartDetailsFromDB == null)
                    {
                        //create cartDetails
                        cartDto.CartDetails.First().CartHeaderId = cartHeaderFromDB.CartHeaderId;
                        await _context.CartDetails.AddAsync(ShoppingCartMapper.MapDtoToCartDetails(cartDto.CartDetails.First()));
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        //Update CartDetails
                        cartDto.CartDetails.First().Count += cartDetailsFromDB.Count;
                        cartDto.CartDetails.First().CartHeaderId = cartDetailsFromDB.CartHeaderId;
                        cartDto.CartDetails.First().CartDetailsId = cartDetailsFromDB.CartDetailsId;
                        _context.CartDetails.Update(ShoppingCartMapper.MapDtoToCartDetails(cartDto.CartDetails.First()));
                        await _context.SaveChangesAsync();
                    }



                }
                _response.Data = cartDto;
            }
            catch (System.Exception ex)
            {

                _response.Message = ex.Message;
                _response.Message = ex.ToString();
                _response.isSuccess = false;
            }
            return _response;
        }

        [HttpPost("RemoveCart")]
        public async Task<ResponseDto> RemoveCart([FromBody] int cartDetailsId)
        {
            try
            {
                var CartDetailsFromDB = await _context.CartDetails.FirstOrDefaultAsync(cd => cd.CartDetailsId == cartDetailsId);
                _context.CartDetails.Remove(CartDetailsFromDB);
                var totalCartDetailscount = _context.CartDetails.Where(cd => cd.CartHeaderId == CartDetailsFromDB.CartHeaderId).Count();
                if (totalCartDetailscount == 1)
                {
                    var cartHeaderToRemove = await _context.CartHeaders.FirstAsync(ch => ch.CartHeaderId == CartDetailsFromDB.CartHeaderId);
                    _context.CartHeaders.Remove(cartHeaderToRemove);


                }
                await _context.SaveChangesAsync();

                _response.Data = true;
            }
            catch (System.Exception ex)
            {

                _response.Message = ex.Message;
                _response.isSuccess = false;
            }
            return _response;
        }

    }
}