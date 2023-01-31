using Microsoft.EntityFrameworkCore;
using Shop.Entities.ShopEntities;
using Shop.Infrastructures;
using Shop.Services.ShopServices.ProductServices.Contracts;
using Shop.Services.ShopServices.ProductServices.Contracts.Dtos;

namespace Shop.Persistence.EF.ShopRepositories.Products
{
    public class EFProductRepository : ProductRepository
    {
        private readonly DbSet<Product> _products;

        public EFProductRepository(EFDataContext context)
        {
            _products = context.Set<Product>();
        }

        public void Add(Product product)
        {
            _products.Add(product);
        }

        public async Task<IPageResult<GetAllProductsDto>> GetAll(
            ISort<GetAllProductsDto>? sortExpression,
            Pagination? pagination,
            string? search)
        {
            var products = GetProducts();

            products = DoSearchOnProducts(search, products);

            if (sortExpression != null) products = products.Sort(sortExpression);

            if (pagination != null)
            {
                var finalResult = await products.Page(pagination).ToListAsync();
                return new PageResult<GetAllProductsDto>(
                    finalResult, finalResult.Count);
            }

            return new PageResult<GetAllProductsDto>(
                products, products.ToList().Count);
        }

        public async Task<bool> IsAnyExistByTitle(string title)
        {
            return await _products.AnyAsync(_ => _.Title == title);
        }

        private IQueryable<GetAllProductsDto> GetProducts()
        {
            return _products
            .Select(_ => new GetAllProductsDto
            {
                Id = _.Id,
                Title = _.Title,
                Description = _.Description,
                Price = _.Price,
                CreationDate = _.CreationDate,
            }).OrderByDescending(_ => _.CreationDate);
        }

        private IQueryable<GetAllProductsDto> DoSearchOnProducts(
            string? search, 
            IQueryable<GetAllProductsDto> products)
        {
            if (!string.IsNullOrWhiteSpace(search))
            {
                products = products.Where(_ =>
                    _.Title.Contains(search) ||
                    _.Description.Contains(search));
            }

            return products;
        }
    }
}
