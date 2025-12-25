using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingMicroservices.Services.ShoppingCartAPI.Data;
using ShoppingMicroservices.Services.ShoppingCartAPI.Mapper;
using ShoppingMicroservices.Services.ShoppingCartAPI.Models.Dto;

namespace ShoppingMicroservices.Services.ShoppingCartAPI.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ResponseDto _response;
        public CartController(AppDbContext context)
        {
            this._context = context;
            _response = new();
        }

        [HttpPost("CartUpSert")]
        public async Task<ResponseDto> CartUpSert(CartDto cartDto)
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
                _response.isSuccess = false;
            }
            return _response;
        }

    }
}