namespace Shop.Services.ShopServices.ProductServices.Contracts.Dtos
{
    public class GetProductDto
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string ImageId { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
