using dotnet_inventory_example.Models;
using GridMvc.Server;
using GridShared;
using GridShared.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using System;
using System.Threading.Tasks;

namespace dotnet_inventory_example.Services
{
    public class Product2Service : IProductService2
    {
        private readonly DbContextOptions<NorthwindDbContext> _options;

        public Product2Service(DbContextOptions<NorthwindDbContext> options)
        {
            _options = options;
        }

        public async Task<ItemsDTO<Product2>> GetProduct2sGridRowsAsync(Action<IGridColumnCollection<Product2>> columns,
            QueryDictionary<StringValues> query)
        {
            using (var context = new NorthwindDbContext(_options))
            {
                var repository = new ProductsRepository2(context);
                var server = new GridServer<Product2>(repository.GetAll(), new QueryCollection(query),
                    true, "Product2sGrid", columns)
                        .Sortable()
                        .WithPaging(10)
                        .Filterable()
                        .WithMultipleFilters()
                        .Groupable(true)
                        .Searchable(true, false, false)
                        .SetRemoveDiacritics<NorthwindDbContext>("RemoveDiacritics");

                // return items to displays
                var items = await server.GetItemsToDisplayAsync(async x => await x.ToListAsync());
                return items;
            }
        }

        public async Task<Product2> Get(params object[] keys)
        {
            using (var context = new NorthwindDbContext(_options))
            {
                int Product2Id;
                int.TryParse(keys[0].ToString(), out Product2Id);
                var repository = new ProductsRepository2(context);
                return await repository.GetById(Product2Id);
            }
        }

        public async Task Insert(Product2 item)
        {
            using (var context = new NorthwindDbContext(_options))
            {
                var repository = new ProductsRepository2(context);
                await repository.Insert(item);
                repository.Save();
            }
        }

        public async Task Update(Product2 item)
        {
            using (var context = new NorthwindDbContext(_options))
            {
                var repository = new ProductsRepository2(context);
                await repository.Update(item);
                repository.Save();
            }
        }

        public async Task Delete(params object[] keys)
        {
            using (var context = new NorthwindDbContext(_options))
            {
                var Product2 = await Get(keys);
                var repository = new ProductsRepository2(context);
                repository.Delete(Product2);
                repository.Save();
            }
        }
    }

    public interface IProductService2 : ICrudDataService<Product2>
    {
        Task<ItemsDTO<Product2>> GetProduct2sGridRowsAsync(Action<IGridColumnCollection<Product2>> columns, QueryDictionary<StringValues> query);
    }
}
