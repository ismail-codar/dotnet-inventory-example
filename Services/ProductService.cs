using dotnet_inventory_example.Models;
using GridMvc.Server;
using GridShared;
using GridShared.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace dotnet_inventory_example.Services
{
    public class ProductService : IProductService
    {
        private readonly DbContextOptions<InventoryDbContext> _options;

        public ProductService(DbContextOptions<InventoryDbContext> options)
        {
            _options = options;
        }

        public async Task<ItemsDTO<Product>> GetProductsGridRowsAsync(Action<IGridColumnCollection<Product>> columns,
            QueryDictionary<StringValues> query)
        {
            using (var context = new InventoryDbContext(_options))
            {
                var repository = new ProductsRepository(context);
                var server = new GridServer<Product>(repository.GetAll(), new QueryCollection(query),
                    true, "ProductsGrid", columns)
                        .Sortable()
                        .WithPaging(10)
                        .Filterable()
                        .WithMultipleFilters()
                        .Searchable(true, false, false)
                        .SetRemoveDiacritics<InventoryDbContext>("RemoveDiacritics");

                // return items to displays
                var items = await server.GetItemsToDisplayAsync(async x => await x.ToListAsync());
                return items;
            }
        }

        public async Task<Product> Get(params object[] keys)
        {
            using (var context = new InventoryDbContext(_options))
            {
                int ProductId;
                int.TryParse(keys[0].ToString(), out ProductId);
                var repository = new ProductsRepository(context);
                return await repository.GetById(ProductId);
            }
        }

        public async Task Insert(Product item)
        {
            using (var context = new InventoryDbContext(_options))
            {
                var repository = new ProductsRepository(context);
                await repository.Insert(item);
                repository.Save();
            }
        }

        public async Task Update(Product item)
        {
            using (var context = new InventoryDbContext(_options))
            {
                var repository = new ProductsRepository(context);
                await repository.Update(item);
                repository.Save();
            }
        }

        public async Task Delete(params object[] keys)
        {
            using (var context = new InventoryDbContext(_options))
            {
                var Product = await Get(keys);
                var repository = new ProductsRepository(context);
                repository.Delete(Product);
                repository.Save();
            }
        }

        public IEnumerable<SelectItem> GetAllProducts()
        {
            using (var context = new InventoryDbContext(_options))
            {
                ProductsRepository repository = new ProductsRepository(context);
                var list = repository.GetAll()
                    .Select(r => new SelectItem(r.ProductId + "", r.ProductName))
                    .ToList();
                return list;
            }
        }
    }

    public interface IProductService : ICrudDataService<Product>
    {
        IEnumerable<SelectItem> GetAllProducts();
        Task<ItemsDTO<Product>> GetProductsGridRowsAsync(Action<IGridColumnCollection<Product>> columns, QueryDictionary<StringValues> query);
    }
}
