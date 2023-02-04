using System.ComponentModel.DataAnnotations;

namespace Shop.Services.ShopServices.ProductServices.Contracts.Dtos
{
    public class EditProductDto
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public double Price { get; set; }
    }
}
