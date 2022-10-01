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
        private readonly DbContextOptions<InventoryDbContext> _options;

        public WorkOrderService(DbContextOptions<InventoryDbContext> options)
        {
            _options = options;
        }

        public async Task<ItemsDTO<WorkOrder>> GetsGridRowsAsync(Action<IGridColumnCollection<WorkOrder>> columns,
            QueryDictionary<StringValues> query)
        {
            using (var context = new InventoryDbContext(_options))
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
                        .SetRemoveDiacritics<InventoryDbContext>("RemoveDiacritics");

                // return items to displays
                var items = await server.GetItemsToDisplayAsync(async x => await x.ToListAsync());
                return items;
            }
        }

        public async Task<WorkOrder> Get(params object[] keys)
        {
            using (var context = new InventoryDbContext(_options))
            {
                int WorkOrderId;
                int.TryParse(keys[0].ToString(), out WorkOrderId);
                var repository = new WorkOrderRepository(context);
                return await repository.GetById(WorkOrderId);
            }
        }

        public async Task Insert(WorkOrder item)
        {
            using (var context = new InventoryDbContext(_options))
            {
                var repository = new WorkOrderRepository(context);
                await this.workOrderInsertProcuctStockChanges(context, item);
                await repository.Insert(item);
                repository.Save();
            }
        }

        public async Task Update(WorkOrder item)
        {
            using (var context = new InventoryDbContext(_options))
            {
                var repository = new WorkOrderRepository(context);
                await this.workOrderUpdateProcuctStockChanges(context, item);
                await repository.Update(item);
                repository.Save();
            }
        }

        public async Task Delete(params object[] keys)
        {
            using (var context = new InventoryDbContext(_options))
            {
                var dataItem = await Get(keys);
                var repository = new WorkOrderRepository(context);
                await this.workOrderDeleteProcuctStockChanges(context, keys);
                repository.Delete(dataItem);
                repository.Save();
            }
        }

        public async Task upsertProductStock(InventoryDbContext context, int productId, int roomId, int quantity)
        {
            await context.ProductStock.Upsert(new ProductStock // insert
            {
                ProductId = productId,
                StockRoomId = roomId,
                Quantity = quantity
            })
            .On(productStock => new { productStock.StockRoomId, productStock.ProductId }) // conflict durumunda -> StockRoomId, ProductId kaydı var ise 
            .WhenMatched(productStock => new ProductStock
            {
                Quantity = productStock.Quantity + quantity // var olan kaydın quantity değerini arttır
            })
            .RunAsync();
        }

        public async Task workOrderInsertProcuctStockChanges(InventoryDbContext context, WorkOrder workOrder)
        {
            if (workOrder.SourceRoomId != null)
            {
                await upsertProductStock(context: context,
                                   productId: workOrder.ProductId,
                                   roomId: (int)workOrder.SourceRoomId,
                                   quantity: workOrder.Quantity);
            }
            if (workOrder.TargetRoomId != null)
            {
                await upsertProductStock(context: context,
                                   productId: workOrder.ProductId,
                                   roomId: (int)workOrder.TargetRoomId,
                                   quantity: workOrder.Quantity);
            }
        }

        public Task workOrderUpdateProcuctStockChanges(InventoryDbContext context, WorkOrder workOrder)
        {
            if (workOrder.SourceRoomId != null)
            {

            }
            if (workOrder.TargetRoomId != null)
            {

            }
            throw new NotImplementedException();
        }

        public async Task workOrderDeleteProcuctStockChanges(InventoryDbContext context, params object[] keys)
        {
            WorkOrder workOrder = await Get(keys);
            if (workOrder.SourceRoomId != null)
            {

            }
            if (workOrder.TargetRoomId != null)
            {

            }
            throw new NotImplementedException();
        }
    }

    public interface IWorkOrderService : ICrudDataService<WorkOrder>
    {
        Task<ItemsDTO<WorkOrder>> GetsGridRowsAsync(Action<IGridColumnCollection<WorkOrder>> columns, QueryDictionary<StringValues> query);
        Task workOrderInsertProcuctStockChanges(InventoryDbContext context, WorkOrder workOrder);
        Task workOrderUpdateProcuctStockChanges(InventoryDbContext context, WorkOrder workOrder);
        Task workOrderDeleteProcuctStockChanges(InventoryDbContext context, params object[] keys);
    }
}
