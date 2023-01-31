namespace Shop.Entities.ShopEntities
{
    public class ProductProperty
    {
        public string Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public string ProductId { get; set; }
        public Product Product { get; set; }
    }
}
