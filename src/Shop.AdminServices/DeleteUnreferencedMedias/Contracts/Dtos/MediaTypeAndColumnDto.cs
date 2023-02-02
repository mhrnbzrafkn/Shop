namespace Shop.AdminServices.DeleteUnreferencedMedias.Contracts.Dtos
{
    public class MediaTypeAndColumnDto
    {
        public Type? Type { get; set; }
        public string? ColumnSpecifier { get; set; }
    }
}