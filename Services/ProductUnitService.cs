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
    public class ProductUnitService : IProductUnitService
    {
        private readonly DbContextOptions<InventoryDbContext> _options;

        public ProductUnitService(DbContextOptions<InventoryDbContext> options)
        {
            _options = options;
        }

        public async Task<ItemsDTO<ProductUnit>> GetProductUnitGridRowsAsync(Action<IGridColumnCollection<ProductUnit>> columns,
            QueryDictionary<StringValues> query)
        {
            using (var context = new InventoryDbContext(_options))
            {
                var repository = new ProductUnitRepository(context);
                var server = new GridServer<ProductUnit>(repository.GetAll(), new QueryCollection(query),
                    true, "ProductUnitGrid", columns)
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

        public async Task<ProductUnit> Get(params object[] keys)
        {
            using (var context = new InventoryDbContext(_options))
            {
                int UnitId;
                int.TryParse(keys[0].ToString(), out UnitId);
                var repository = new ProductUnitRepository(context);
                return await repository.GetById(UnitId);
            }
        }

        public async Task Insert(ProductUnit item)
        {
            using (var context = new InventoryDbContext(_options))
            {
                var repository = new ProductUnitRepository(context);
                await repository.Insert(item);
                repository.Save();
            }
        }

        public async Task Update(ProductUnit item)
        {
            using (var context = new InventoryDbContext(_options))
            {
                var repository = new ProductUnitRepository(context);
                await repository.Update(item);
                repository.Save();
            }
        }

        public async Task Delete(params object[] keys)
        {
            using (var context = new InventoryDbContext(_options))
            {
                var Unit = await Get(keys);
                var repository = new ProductUnitRepository(context);
                repository.Delete(Unit);
                repository.Save();
            }
        }

        public IEnumerable<SelectItem> GetAllProductUnit()
        {
            using (var context = new InventoryDbContext(_options))
            {
                ProductUnitRepository repository = new ProductUnitRepository(context);
                var list = repository.GetAll()
                    .Select(r => new SelectItem(r.UnitId + "", r.UnitName))
                    .ToList();
                return list;
            }
        }
    }

    public interface IProductUnitService : ICrudDataService<ProductUnit>
    {
        IEnumerable<SelectItem> GetAllProductUnit();
        Task<ItemsDTO<ProductUnit>> GetProductUnitGridRowsAsync(Action<IGridColumnCollection<ProductUnit>> columns, QueryDictionary<StringValues> query);
    }
}
