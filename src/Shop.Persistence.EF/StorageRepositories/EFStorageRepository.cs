using Microsoft.EntityFrameworkCore;
using Shop.Entities.StorageEntities;
using Shop.Services.StorageServices.Contracts;
using Shop.Services.StorageServices.Contracts.Dtos;

namespace Shop.Persistence.EF.StorageRepositories
{
    public class EFStorageRepository : StorageRepository
    {
        private readonly DbSet<Media> _documents;

        public EFStorageRepository(EFDataContext cmsDb)
        {
            _documents = cmsDb.Set<Media>();
        }

        public void Add(Media media)
        {
            _documents.Add(media);
        }

        public GetMediaDto? GetById(string id)
        {
            return _documents.Where(_ => _.Id == id)
            .Select(document => new GetMediaDto
            {
                Data = document.Data,
                Extension = document.Extension
            }).SingleOrDefault();
        }
    }
}
