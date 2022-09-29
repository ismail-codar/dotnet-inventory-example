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
    public class UnitService : IUnitService
    {
        private readonly DbContextOptions<NorthwindDbContext> _options;

        public UnitService(DbContextOptions<NorthwindDbContext> options)
        {
            _options = options;
        }

        public async Task<ItemsDTO<Units>> GetUnitsGridRowsAsync(Action<IGridColumnCollection<Units>> columns,
            QueryDictionary<StringValues> query)
        {
            using (var context = new NorthwindDbContext(_options))
            {
                var repository = new UnitsRepository(context);
                var server = new GridServer<Units>(repository.GetAll(), new QueryCollection(query),
                    true, "UnitsGrid", columns)
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

        public async Task<Units> Get(params object[] keys)
        {
            using (var context = new NorthwindDbContext(_options))
            {
                int UnitId;
                int.TryParse(keys[0].ToString(), out UnitId);
                var repository = new UnitsRepository(context);
                return await repository.GetById(UnitId);
            }
        }

        public async Task Insert(Units item)
        {
            using (var context = new NorthwindDbContext(_options))
            {
                var repository = new UnitsRepository(context);
                await repository.Insert(item);
                repository.Save();
            }
        }

        public async Task Update(Units item)
        {
            using (var context = new NorthwindDbContext(_options))
            {
                var repository = new UnitsRepository(context);
                await repository.Update(item);
                repository.Save();
            }
        }

        public async Task Delete(params object[] keys)
        {
            using (var context = new NorthwindDbContext(_options))
            {
                var Unit = await Get(keys);
                var repository = new UnitsRepository(context);
                repository.Delete(Unit);
                repository.Save();
            }
        }

        public IEnumerable<SelectItem> GetAllUnits()
        {
            using (var context = new NorthwindDbContext(_options))
            {
                UnitsRepository repository = new UnitsRepository(context);
                var list = repository.GetAll()
                    .Select(r => new SelectItem(r.UnitId + "", r.UnitName))
                    .ToList();
                return list;
            }
        }
    }

    public interface IUnitService : ICrudDataService<Units>
    {
        IEnumerable<SelectItem> GetAllUnits();
        Task<ItemsDTO<Units>> GetUnitsGridRowsAsync(Action<IGridColumnCollection<Units>> columns, QueryDictionary<StringValues> query);
    }
}
