using Shop.Infrastructures.Storages;

namespace Shop.Entities.ShopEntities
{
    public class Product
    {
        public Product()
        {
            CreationDate = DateTime.UtcNow;
            Properties = new List<ProductProperty>();
        }

        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public Media Image { get; set; }
        public DateTime CreationDate { get; set; }
        public List<ProductProperty> Properties { get; set; }
    }
}
