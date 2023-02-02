namespace Shop.Services.StorageServices.Contracts.UnRefrencedDocumentDtos
{
    public class DeleteUnRefrencedMediaDto
    {
        public List<string> DocumentDeleted { get; set; } = new();
        public List<MediaNotDeletedDto> DocumentsNotDeleted { get; set; } = new();
    }
}
