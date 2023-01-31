using Microsoft.EntityFrameworkCore;
using Shop.Entities.ShopEntities;
using Shop.Infrastructures;
using Shop.Services.ShopServices.ProductPropertyServices.Contracts;
using Shop.Services.ShopServices.ProductPropertyServices.Contracts.Dtos;

namespace Shop.Persistence.EF.ShopRepositories.ProductProperties
{
    public class EFProductPropertyRepository : ProductPropertyRepository
    {
        private readonly DbSet<ProductProperty> _productProperties;

        public EFProductPropertyRepository(EFDataContext context)
        {
            _productProperties = context.Set<ProductProperty>();
        }

        public void Add(ProductProperty productProperty)
        {
            _productProperties.Add(productProperty);
        }

        public async Task<IPageResult<GetAllProductPropertiesDto>> GetAll(
            string productId,
            ISort<GetAllProductPropertiesDto>? sortExpression,
            Pagination? pagination,
            string? search)
        {
            var productproperties = GetProductProperties(productId);

            productproperties = DoSearchOnProducts(search, productproperties);

            if (sortExpression != null) productproperties = productproperties.Sort(sortExpression);

            if (pagination != null)
            {
                var finalResult = await productproperties.Page(pagination).ToListAsync();
                return new PageResult<GetAllProductPropertiesDto>(
                    finalResult, finalResult.Count);
            }

            return new PageResult<GetAllProductPropertiesDto>(
                productproperties, productproperties.ToList().Count);
        }

        public void Delete(ProductProperty productProperty)
        {
            _productProperties.Remove(productProperty);
        }

        public async Task<ProductProperty> Find(string id)
        {
            return await _productProperties.FindAsync(id);
        }

        public async Task<bool> IsKeyDuplicated(
            string productId,
            string key)
        {
            return await _productProperties.AnyAsync(_ =>
            _.ProductId == productId && _.Key.Trim().ToLower() == key.Trim().ToLower());
        }

        private IQueryable<GetAllProductPropertiesDto> DoSearchOnProducts(
            string? search,
            IQueryable<GetAllProductPropertiesDto> productProperties)
        {
            if (!string.IsNullOrWhiteSpace(search))
            {
                productProperties = productProperties.Where(_ =>
                    _.Key.Contains(search) ||
                    _.Value.Contains(search));
            }

            return productProperties;
        }

        private IQueryable<GetAllProductPropertiesDto> GetProductProperties(
            string productId)
        {
            return _productProperties
                .Where(_ => _.ProductId == productId)
                .Select(_ => new GetAllProductPropertiesDto
                {
                    Id = _.Id,
                    Key = _.Key,
                    Value = _.Value,
                });
        }
    }
}
