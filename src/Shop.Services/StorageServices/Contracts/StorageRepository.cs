using Shop.Entities.StorageEntities;
using Shop.Infrastructures;
using Shop.Services.StorageServices.Contracts.Dtos;

namespace Shop.Services.StorageServices.Contracts
{
    public interface StorageRepository : Repository
    {
        GetMediaDto? GetById(string id);
        void Add(Media document);
    }
}
