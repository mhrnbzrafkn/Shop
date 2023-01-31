using Shop.Entities.ShopEntities;
using Shop.Infrastructures;
using Shop.Services.ShopServices.ProductServices.Contracts;
using Shop.Services.ShopServices.ProductServices.Contracts.Dtos;
using Shop.Services.ShopServices.ProductServices.Exceptions;

namespace Shop.Services.ShopServices.ProductServices
{
    public class ProductAppService : ProductService
    {
        private readonly ProductRepository _repository;
        private readonly UnitOfWork _unitOfWork;

        public ProductAppService(
            ProductRepository repository,
            UnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<string> Add(AddProductDto dto)
        {
            await StopIfTitleIsDuplicated(dto.Title);

            StopIfPriceIsNotValid(dto.Price);

            var product = new Product
            {
                Title = dto.Title,
                Description = dto.Description,
                Price = dto.Price
            };

            _repository.Add(product);

            await _unitOfWork.Complete();

            return product.Id;
        }

        private async Task StopIfTitleIsDuplicated(string title)
        {
            bool isAnyExistByTitle = await _repository.IsAnyExistByTitle(title);
            if (isAnyExistByTitle)
                throw new DuplicatedProductException();
        }

        private void StopIfPriceIsNotValid(double price)
        {
            if (price < 0)
                throw new ProductPriceIsNotValidException();
        }
    }
}