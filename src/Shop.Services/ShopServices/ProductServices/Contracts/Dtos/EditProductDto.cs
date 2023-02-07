﻿using System.ComponentModel.DataAnnotations;

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
        [Required]
        [Range(0, 90)]
        public double Longitude { get; set; }
        [Required]
        [Range(0, 90)]
        public double Latitude { get; set; }
    }
}
