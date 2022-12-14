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
    public class StockRoomService : IStockRoomService
    {
        private readonly DbContextOptions<InventoryDbContext> _options;

        public StockRoomService(DbContextOptions<InventoryDbContext> options)
        {
            _options = options;
        }

        public async Task<ItemsDTO<StockRoom>> GetsGridRowsAsync(Action<IGridColumnCollection<StockRoom>> columns,
            QueryDictionary<StringValues> query)
        {
            using (var context = new InventoryDbContext(_options))
            {
                var repository = new StockRoomRepository(context);
                var server = new GridServer<StockRoom>(repository.GetAll(), new QueryCollection(query),
                    true, "StockRoomsGrid", columns)
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

        public async Task<StockRoom> Get(params object[] keys)
        {
            using (var context = new InventoryDbContext(_options))
            {
                int StockRoomId;
                int.TryParse(keys[0].ToString(), out StockRoomId);
                var repository = new StockRoomRepository(context);
                return await repository.GetById(StockRoomId);
            }
        }

        public async Task Insert(StockRoom item)
        {
            using (var context = new InventoryDbContext(_options))
            {
                var repository = new StockRoomRepository(context);
                await repository.Insert(item);
                repository.Save();
            }
        }

        public async Task Update(StockRoom item)
        {
            using (var context = new InventoryDbContext(_options))
            {
                var repository = new StockRoomRepository(context);
                await repository.Update(item);
                repository.Save();
            }
        }

        public async Task Delete(params object[] keys)
        {
            using (var context = new InventoryDbContext(_options))
            {
                var dataItem = await Get(keys);
                var repository = new StockRoomRepository(context);
                repository.Delete(dataItem);
                repository.Save();
            }
        }

        public IEnumerable<SelectItem> GetAllStockRooms()
        {
            using (var context = new InventoryDbContext(_options))
            {
                StockRoomRepository repository = new StockRoomRepository(context);
                var list = repository.GetAll()
                    .Select(r => new SelectItem(r.StockRoomId + "", r.StockBuilding.BuildingName + " - " + r.RoomName))
                    .ToList();
                return list;
            }
        }
    }

    public interface IStockRoomService : ICrudDataService<StockRoom>
    {

        IEnumerable<SelectItem> GetAllStockRooms();
        Task<ItemsDTO<StockRoom>> GetsGridRowsAsync(Action<IGridColumnCollection<StockRoom>> columns, QueryDictionary<StringValues> query);
    }
}
