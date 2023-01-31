using Shop.Entities.ShopEntities;
using Shop.Infrastructures;
using Shop.Services.ShopServices.ProductPropertyServices.Contracts;
using Shop.Services.ShopServices.ProductPropertyServices.Contracts.Dtos;
using Shop.Services.ShopServices.ProductPropertyServices.Exceptions;
using Shop.Services.ShopServices.ProductServices.Contracts;
using Shop.Services.ShopServices.ProductServices.Exceptions;

namespace Shop.Services.ShopServices.ProductPropertyServices
{
    public class ProductPropertyAppService : ProductPropertyService
    {
        private readonly ProductPropertyRepository _repository;
        private readonly ProductRepository _productRepository;
        private readonly UnitOfWork _unitOfWork;

        public ProductPropertyAppService(
            ProductPropertyRepository repository,
            UnitOfWork unitOfWork,
            ProductRepository productRepository)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _productRepository = productRepository;
        }

        public async Task<string> Add(string productId, AddProductPropertyDto dto)
        {
            await StopIfProductIsNotExist(productId);

            await StopIfPropertyKeyIsDuplicated(productId, dto.Key);

            var productProperty = new ProductProperty
            {
                ProductId = productId,
                Key = dto.Key,
                Value = dto.Value,
            };

            _repository.Add(productProperty);

            await _unitOfWork.Complete();

            return productProperty.Id;
        }

        public async Task Delete(string id)
        {
            var productProperty = await _repository.Find(id);

            StopIfProductPropertyNotFound(productProperty);

            _repository.Delete(productProperty);

            await _unitOfWork.Complete();
        }

        public async Task<IPageResult<GetAllProductPropertiesDto>> GetAll(
            string productId,
            ISort<GetAllProductPropertiesDto>? sortExpression,
            Pagination? pagination,
            string? search)
        {
            return await _repository.GetAll(
                productId,
                sortExpression,
                pagination,
                search);
        }

        private async Task StopIfProductIsNotExist(string productId)
        {
            var isProductExist = await _productRepository.FindById(productId);
            if (isProductExist == null)
                throw new ProductNotFoundException();
        }

        private async Task StopIfPropertyKeyIsDuplicated(
            string productId, 
            string key)
        {
            bool isKeyDuplicated = await _repository.IsKeyDuplicated(productId, key);
            if (isKeyDuplicated)
                throw new DuplicatedPropertyKeyException();
        }

        private static void StopIfProductPropertyNotFound(ProductProperty productProperty)
        {
            if (productProperty == null)
                throw new ProductPropertyNotFoundException();
        }
    }
}
