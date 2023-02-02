namespace Shop.Entities.StorageEntities
{
    public class Media
    {
        public string Id { get; set; }
        public byte[] Data { get; set; }
        public DateTime CreationDate { get; set; }
        public string? FileName { get; set; }
        public string? Extension { get; set; }
    }
}
