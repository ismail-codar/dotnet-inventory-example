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
    public class StockBuildingService : IStockBuildingService
    {
        private readonly DbContextOptions<NorthwindDbContext> _options;

        public StockBuildingService(DbContextOptions<NorthwindDbContext> options)
        {
            _options = options;
        }

        public async Task<ItemsDTO<StockBuilding>> GetsGridRowsAsync(Action<IGridColumnCollection<StockBuilding>> columns,
            QueryDictionary<StringValues> query)
        {
            using (var context = new NorthwindDbContext(_options))
            {
                var repository = new StockBuildingRepository(context);
                var server = new GridServer<StockBuilding>(repository.GetAll(), new QueryCollection(query),
                    true, "StockBuildingsGrid", columns)
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

        public async Task<StockBuilding> Get(params object[] keys)
        {
            using (var context = new NorthwindDbContext(_options))
            {
                int StockBuildingId;
                int.TryParse(keys[0].ToString(), out StockBuildingId);
                var repository = new StockBuildingRepository(context);
                return await repository.GetById(StockBuildingId);
            }
        }

        public async Task Insert(StockBuilding item)
        {
            using (var context = new NorthwindDbContext(_options))
            {
                var repository = new StockBuildingRepository(context);
                await repository.Insert(item);
                repository.Save();
            }
        }

        public async Task Update(StockBuilding item)
        {
            using (var context = new NorthwindDbContext(_options))
            {
                var repository = new StockBuildingRepository(context);
                await repository.Update(item);
                repository.Save();
            }
        }

        public async Task Delete(params object[] keys)
        {
            using (var context = new NorthwindDbContext(_options))
            {
                var dataItem = await Get(keys);
                var repository = new StockBuildingRepository(context);
                repository.Delete(dataItem);
                repository.Save();
            }
        }
    }

    public interface IStockBuildingService : ICrudDataService<StockBuilding>
    {
        Task<ItemsDTO<StockBuilding>> GetsGridRowsAsync(Action<IGridColumnCollection<StockBuilding>> columns, QueryDictionary<StringValues> query);
    }
}
