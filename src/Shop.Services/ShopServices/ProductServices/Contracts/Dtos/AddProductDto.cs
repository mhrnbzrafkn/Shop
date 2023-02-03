using System.ComponentModel.DataAnnotations;

namespace Shop.Services.ShopServices.ProductServices.Contracts.Dtos
{
    public class AddProductDto
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public string ImageId { get; set; }
    }
}
