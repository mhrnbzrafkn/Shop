﻿using Microsoft.EntityFrameworkCore;
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

        public void Delete(Product product)
        {
            _products.Remove(product);
        }

        public async Task<GetProductDto?> Get(string id)
        {
            return await _products
            .Select(_ => new GetProductDto
            {
                Id = _.Id,
                Title = _.Title,
                Description = _.Description,
                Price = _.Price,
                ImageId = _.Image.Id,
                CreationDate = _.CreationDate,
                Longitude = _.Longitude,
                Latitude = _.Latitude

            }).SingleOrDefaultAsync(_ => _.Id == id);
        }

        public async Task<Product?> FindById(string id)
        {
            return await _products.FindAsync(id);
        }

        public async Task<bool> IsAnyExistByTitle(string title)
        {
            return await _products.AnyAsync(_ => _.Title == title);
        }

        public async Task<bool> IsTitleDuplicated(string id, string title)
        {
            return await _products.AnyAsync(_ => _.Id != id &&
            _.Title.Trim().ToLower() == title.Trim().ToLower());
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
                ImageId = _.Image.Id,
                CreationDate = _.CreationDate,
                Longitude = _.Longitude,
                Latitude = _.Latitude
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
