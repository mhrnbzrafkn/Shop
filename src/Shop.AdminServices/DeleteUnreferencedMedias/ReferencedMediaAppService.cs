using System.Linq.Dynamic.Core;
using Shop.AdminServices.DeleteUnreferencedMedias.Contracts.Dtos;
using Shop.Entities.ShopEntities;
using Shop.Persistence.EF;
using System.Reflection;
using Shop.Infrastructures;

namespace Shop.AdminServices.DeleteUnreferencedMedias;

public class ReferencedMediaAppService
{
    private readonly EFDataContext _dbContext;

    public ReferencedMediaAppService(EFDataContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<string>> FindAll()
    {
        var assembly = Assembly.GetAssembly(typeof(Product));

        var typeWithDocuments =
            assembly?.GetTypes()
                .Where(_ => _.GetProperties()
                    .Any(s => s.PropertyType == typeof(Shop.Entities.StorageEntities.Media)))
                .SelectMany(_ => _.GetProperties()
                    .Where(s => s.PropertyType == typeof(Shop.Entities.StorageEntities.Media))
                    .Select(t => new MediaTypeAndColumnDto
                    {
                        Type = _,
                        ColumnSpecifier = t.Name
                    })).ToList();

        return await FindReferencedDocumentIds(
            typeWithDocuments ?? new List<MediaTypeAndColumnDto>());
    }

    private async Task<List<string>> FindReferencedDocumentIds(
        List<MediaTypeAndColumnDto> documentTypes)
    {
        var referencedDocumentIds
            = Enumerable.Empty<string>();
        foreach (var item in documentTypes)
        {
            referencedDocumentIds = await referencedDocumentIds.Union(
                _dbContext.Set(item.Type)
                .Where($"_ =>  _.{item.ColumnSpecifier} != null")
                .Select($"_ => _.{item.ColumnSpecifier}.Id")
                .ToDynamicList<string>())
                .ToDynamicListAsync<string>();
        }

        return referencedDocumentIds.ToList();
    }
}