namespace Shop.Services.StorageServices.Contracts.UnRefrencedDocumentDtos
{
    public class DeleteUnRefrencedMediaDto
    {
        public List<string> MediasDeleted { get; set; } = new();
        public List<MediaNotDeletedDto> MediasNotDeleted { get; set; } = new();
    }
}
