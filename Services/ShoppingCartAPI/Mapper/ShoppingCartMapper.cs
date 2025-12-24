using Riok.Mapperly.Abstractions;
using ShoppingMicroservices.Services.ProductAPI.Models;
using ShoppingMicroservices.Services.ProductAPI.Models.Dto;
using ShoppingMicroservices.Services.ShoppingCartAPI.Models;
using ShoppingMicroservices.Services.ShoppingCartAPI.Models.Dto;

namespace ShoppingMicroservices.Services.ShoppingCartAPI.Mapper
{
    // Enums of source and target have different numeric values -> use ByName strategy to map them
    [Mapper(EnumMappingStrategy = EnumMappingStrategy.ByName)]
    public static partial class ProductMapper
    {

        // [MapProperty(nameof(coupon.CouponId), nameof(CouponDto.CouponId))]
        public static partial ShoppingCartDto MapShoppingCartToDto(ShoppingCart shoppingCart);
        public static partial IEnumerable<ShoppingCartDto> MapShoppingCartToDto(IEnumerable<ShoppingCart> shoppingCarts);


        public static partial ShoppingCart MapDtoToShoppingCart(ShoppingCartDto shoppingCartDto);
        public static partial IEnumerable<ShoppingCart> MapDtoToShoppingCart(IEnumerable<ShoppingCartDto> shoppingCartDtos);

        // public static partial Coupon MapAddCouponDtoToCoupon(AddCouponDto addCouponDto);

    }
}