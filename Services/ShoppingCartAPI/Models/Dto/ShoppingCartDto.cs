using System.ComponentModel.DataAnnotations;

namespace ShoppingMicroservices.Services.ShoppingCartAPI.Models.Dto
{
    public class ShoppingCartDto
    {
        [Key] public int ShoppingCartId { get; set; }
        [Required] public string Name { get; set; }
        [Range(1, 100)] public double Price { get; set; }
        public string Description { get; set; }
        public string CategoryName { get; set; }
        public string ImageUrl { get; set; }


    }
}