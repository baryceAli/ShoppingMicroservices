using Riok.Mapperly.Abstractions;
using ShoppingMicroservices.Services.ProductAPI.Models;
using ShoppingMicroservices.Services.ProductAPI.Models.Dto;

namespace ShoppingMicroservices.Services.CouponAPI.Mapper
{
    // Enums of source and target have different numeric values -> use ByName strategy to map them
    [Mapper(EnumMappingStrategy = EnumMappingStrategy.ByName)]
    public static partial class ProductMapper
    {

        // [MapProperty(nameof(coupon.CouponId), nameof(CouponDto.CouponId))]
        public static partial ProductDto MapProductToDto(Product product);
        public static partial IEnumerable<ProductDto> MapProductToDto(IEnumerable<Product> products);


        public static partial Product MapDtoToProduct(ProductDto productDto);
        public static partial IEnumerable<Product> MapDtoToProduct(IEnumerable<ProductDto> productDtos);

        // public static partial Coupon MapAddCouponDtoToCoupon(AddCouponDto addCouponDto);

    }
}