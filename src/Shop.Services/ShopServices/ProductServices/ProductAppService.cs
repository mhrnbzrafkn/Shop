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

        public async Task<IPageResult<GetAllProductsDto>> GetAll(
            ISort<GetAllProductsDto>? sortExpression,
            Pagination? pagination,
            string? search)
        {
            return await _repository.GetAll(sortExpression, pagination, search);
        }

        public async Task Delete(string id)
        {
            var product = await _repository.FindById(id);

            StopIfProductIsNotExist(product);

            _repository.Delete(product);

            await _unitOfWork.Complete();
        }

        public async Task<GetProductDto?> Get(string id)
        {
            return await _repository.Get(id);
        }

        public async Task Edit(string id, EditProductDto dto)
        {
            var product = await _repository.FindById(id);

            await StopIfTitleIsDuplicated(id, dto);

            product.Title = dto.Title;
            product.Description = dto.Description;
            product.Price = dto.Price;

            await _unitOfWork.Complete();
        }

        private async Task StopIfTitleIsDuplicated(string id, EditProductDto dto)
        {
            var isTitleDuplicated = await _repository.IsTitleDuplicated(id, dto.Title);
            if (isTitleDuplicated)
                throw new DuplicatedProductTitleException();
        }

        private async Task StopIfTitleIsDuplicated(string title)
        {
            bool isAnyExistByTitle = await _repository.IsAnyExistByTitle(title);
            if (isAnyExistByTitle)
                throw new DuplicatedProductTitleException();
        }

        private static void StopIfProductIsNotExist(Product product)
        {
            if (product == null)
                throw new ProductNotFoundException();
        }

        private void StopIfPriceIsNotValid(double price)
        {
            if (price < 0)
                throw new ProductPriceIsNotValidException();
        }
    }
}