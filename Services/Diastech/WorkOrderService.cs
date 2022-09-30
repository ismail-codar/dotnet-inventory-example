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
    public class WorkOrderService : IWorkOrderService
    {
        private readonly DbContextOptions<NorthwindDbContext> _options;

        public WorkOrderService(DbContextOptions<NorthwindDbContext> options)
        {
            _options = options;
        }

        public async Task<ItemsDTO<WorkOrder>> GetsGridRowsAsync(Action<IGridColumnCollection<WorkOrder>> columns,
            QueryDictionary<StringValues> query)
        {
            using (var context = new NorthwindDbContext(_options))
            {
                var repository = new WorkOrderRepository(context);
                var server = new GridServer<WorkOrder>(repository.GetAll(), new QueryCollection(query),
                    true, "WorkOrdersGrid", columns)
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

        public async Task<WorkOrder> Get(params object[] keys)
        {
            using (var context = new NorthwindDbContext(_options))
            {
                int WorkOrderId;
                int.TryParse(keys[0].ToString(), out WorkOrderId);
                var repository = new WorkOrderRepository(context);
                return await repository.GetById(WorkOrderId);
            }
        }

        public async Task Insert(WorkOrder item)
        {
            using (var context = new NorthwindDbContext(_options))
            {
                var repository = new WorkOrderRepository(context);
                await repository.Insert(item);
                repository.Save();
            }
        }

        public async Task Update(WorkOrder item)
        {
            using (var context = new NorthwindDbContext(_options))
            {
                var repository = new WorkOrderRepository(context);
                await repository.Update(item);
                repository.Save();
            }
        }

        public async Task Delete(params object[] keys)
        {
            using (var context = new NorthwindDbContext(_options))
            {
                var dataItem = await Get(keys);
                var repository = new WorkOrderRepository(context);
                repository.Delete(dataItem);
                repository.Save();
            }
        }
    }

    public interface IWorkOrderService : ICrudDataService<WorkOrder>
    {
        Task<ItemsDTO<WorkOrder>> GetsGridRowsAsync(Action<IGridColumnCollection<WorkOrder>> columns, QueryDictionary<StringValues> query);
    }
}
