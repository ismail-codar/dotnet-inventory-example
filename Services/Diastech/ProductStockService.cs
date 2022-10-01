using dotnet_inventory_example.Models;
using GridMvc.Server;
using GridShared;
using GridShared.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using System;
using System.Threading.Tasks;
using System.Linq;

namespace dotnet_inventory_example.Services
{
    public class ProductStockService : IProductStockService
    {
        private readonly DbContextOptions<NorthwindDbContext> _options;

        public ProductStockService(DbContextOptions<NorthwindDbContext> options)
        {
            _options = options;
        }

        public async Task<ItemsDTO<ProductStock>> GetsGridRowsAsync(Action<IGridColumnCollection<ProductStock>> columns,
            QueryDictionary<StringValues> query)
        {
            using (var context = new NorthwindDbContext(_options))
            {
                var repository = new ProductStockRepository(context);
                var server = new GridServer<ProductStock>(repository.GetAll(), new QueryCollection(query),
                    true, "ProductStocksGrid", columns)
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

        public async Task<ProductStock> Get(params object[] keys)
        {
            using (var context = new NorthwindDbContext(_options))
            {
                int ProductStockId;
                int.TryParse(keys[0].ToString(), out ProductStockId);
                var repository = new ProductStockRepository(context);
                return await repository.GetById(ProductStockId);
            }
        }

        public async Task Insert(ProductStock item)
        {
            using (var context = new NorthwindDbContext(_options))
            {
                var repository = new ProductStockRepository(context);
                await repository.Insert(item);
                repository.Save();
            }
        }

        public async Task Update(ProductStock item)
        {
            using (var context = new NorthwindDbContext(_options))
            {
                var repository = new ProductStockRepository(context);
                await repository.Update(item);
                repository.Save();
            }
        }

        public async Task Delete(params object[] keys)
        {
            using (var context = new NorthwindDbContext(_options))
            {
                var dataItem = await Get(keys);
                var repository = new ProductStockRepository(context);
                repository.Delete(dataItem);
                repository.Save();
            }
        }

        public ProductStock GetProductStockInRoom(int productId, int? targetRoomId)
        {
            using (var context = new NorthwindDbContext(_options))
            {
                var repository = new ProductStockRepository(context);
                var list = repository.GetAll().Where(r => r.ProductId == productId && r.StockRoomId == targetRoomId).ToList();
                return list.Count > 0 ? list[0] : null;
            }
        }
    }

    public interface IProductStockService : ICrudDataService<ProductStock>
    {
        ProductStock GetProductStockInRoom(int productId, int? targetRoomId);
        Task<ItemsDTO<ProductStock>> GetsGridRowsAsync(Action<IGridColumnCollection<ProductStock>> columns, QueryDictionary<StringValues> query);
    }
}
