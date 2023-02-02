using Shop.Infrastructures;
using Shop.Services.StorageServices.Contracts.Dtos;

namespace Shop.Services.StorageServices.Contracts
{
    public interface StorageService : Service
    {
        GetMediaDto? GetById(string id);
        Task<string> Add(AddMediaDto dto);
    }
}
