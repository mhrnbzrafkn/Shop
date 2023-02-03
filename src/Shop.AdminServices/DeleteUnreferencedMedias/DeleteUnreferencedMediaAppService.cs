using Microsoft.EntityFrameworkCore;
using Shop.Entities.StorageEntities;
using Shop.Infrastructures;
using Shop.Persistence.EF;
using Shop.Services.StorageServices.Contracts.UnRefrencedDocumentDtos;

namespace Shop.AdminServices.DeleteUnreferencedMedias;

public class DeleteUnreferencedMediaAppService
{
    private readonly EFDataContext _dbContext;
    private readonly DateTimeService _timeService;
    private readonly ReferencedMediaAppService _referencedDocumentAppService;

    public DeleteUnreferencedMediaAppService(
        EFDataContext dbContext,
        DateTimeService timeService)
    {
        _dbContext = dbContext;
        _timeService = timeService;
        _referencedDocumentAppService = new ReferencedMediaAppService(_dbContext);
    }

    public async Task<DeleteUnRefrencedMediaDto> Execute(CancellationToken stoppingToken)
    {
        var referencedDocuments =
            await _referencedDocumentAppService.FindAll();

        var allDocuments = await GetAllDocuments();

        var unReferencedDocuments = FindUnreferencedDocuments(
            allDocuments,
            referencedDocuments);

        return await DeleteUnReferencedDocuments(
            stoppingToken,
            unReferencedDocuments,
            _timeService.Now);
    }

    private async Task<DeleteUnRefrencedMediaDto>
        DeleteUnReferencedDocuments(
        CancellationToken stoppingToken,
        List<string> unReferencedDocumentIds,
        DateTime now)
    {
        var dto = new DeleteUnRefrencedMediaDto();
        foreach (var unReferencedDocumentId in unReferencedDocumentIds)
        {
            try
            {
                var x = await _dbContext.Database.ExecuteSqlRawAsync(
                $"DELETE FROM Medias WHERE Id =" +
                $"N'{unReferencedDocumentId}' " +
                $"And DATEDIFF(HOUR,CreationDate,N'{now}') > 1",
                stoppingToken);

                dto.MediasDeleted.Add(unReferencedDocumentId);
            }
            catch (Exception e)
            {
                var documentNotDeleted = new MediaNotDeletedDto()
                {
                    MediaNotDeletedId = unReferencedDocumentId,
                    ExceptionMessage = e.Message
                };

                dto.MediasNotDeleted.Add(documentNotDeleted);
            }
        }

        return dto;
    }

    private List<string> FindUnreferencedDocuments(
        List<string> allDocuments,
        List<string> referencedDocuments)
    {
        return allDocuments.Except(referencedDocuments).ToList();
    }

    private async Task<List<string>> GetAllDocuments()
    {
        return await _dbContext.Set<Media>().Select(_ => _.Id).ToListAsync();
    }
}
