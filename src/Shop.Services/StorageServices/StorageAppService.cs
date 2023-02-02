using Shop.Entities.StorageEntities;
using Shop.Infrastructures;
using Shop.Services.StorageServices.Contracts;
using Shop.Services.StorageServices.Contracts.Dtos;

namespace Shop.Services.StorageServices
{
    public class StorageAppService : StorageService
    {
        private readonly StorageRepository _repository;
        private readonly UnitOfWork _unitOfWork;

        public StorageAppService(
            StorageRepository repository,
            UnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public GetMediaDto? GetById(string id)
        {
            return _repository.GetById(id);
        }

        public async Task<string> Add(AddMediaDto dto)
        {
            var id = Guid.NewGuid().ToString();

            var document = new Media
            {
                Id = id,
                CreationDate = DateTime.UtcNow,
                Data = dto.Data,
                FileName = id,
                Extension = dto.Extension.TrimStart('.')
            };

            _repository.Add(document);

            await _unitOfWork.Complete();

            return document.Id;
        }
    }
}
