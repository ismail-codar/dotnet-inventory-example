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
    public class WorkOrderProductService : IWorkOrderProductService
    {
        private readonly DbContextOptions<InventoryDbContext> _options;

        public WorkOrderProductService(DbContextOptions<InventoryDbContext> options)
        {
            _options = options;
        }

        public async Task<ItemsDTO<WorkOrderProduct>> GetsGridRowsAsync(Action<IGridColumnCollection<WorkOrderProduct>> columns,
            QueryDictionary<StringValues> query)
        {
            using (var context = new InventoryDbContext(_options))
            {
                var repository = new WorkOrderProductRepository(context);
                var server = new GridServer<WorkOrderProduct>(repository.GetAll(), new QueryCollection(query),
                    true, "WorkOrderProductsGrid", columns)
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

        public async Task<WorkOrderProduct> Get(params object[] keys)
        {
            using (var context = new InventoryDbContext(_options))
            {
                int WorkOrderProductId;
                int.TryParse(keys[0].ToString(), out WorkOrderProductId);
                var repository = new WorkOrderProductRepository(context);
                return await repository.GetById(WorkOrderProductId);
            }
        }

        public async Task Insert(WorkOrderProduct item)
        {
            using (var context = new InventoryDbContext(_options))
            {
                var repository = new WorkOrderProductRepository(context);
                await this.workOrderInsertProcuctStockChanges(context, item);
                await repository.Insert(item);
                repository.Save();
            }
        }

        public async Task Update(WorkOrderProduct item)
        {
            using (var context = new InventoryDbContext(_options))
            {
                var repository = new WorkOrderProductRepository(context);
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
                var repository = new WorkOrderProductRepository(context);
                await this.workOrderDeleteProcuctStockChanges(context, keys);
                repository.Delete(dataItem);
                repository.Save();
            }
        }


        public async Task upsertProductStock(InventoryDbContext context, int productId, int roomId, int quantity)
        {
            await context.ProductStocks.Upsert(new ProductStock // insert
            {
                ProductId = productId,
                StockRoomId = roomId,
                Quantity = quantity
            })
            .On(productStock => new { productStock.StockRoomId, productStock.ProductId }) // conflict durumunda -> StockRoomId, ProductId kaydı var ise 
            .WhenMatched(productStock => new ProductStock
            {
                Quantity = productStock.Quantity + quantity // var olan kaydın quantity değerini güncele
            })
            .RunAsync();
        }

        public async Task workOrderInsertProcuctStockChanges(InventoryDbContext context, WorkOrderProduct workOrderProduct)
        {
            var workOrderRepository = new WorkOrderRepository(context);
            WorkOrder workOrder = await workOrderRepository.GetById(workOrderProduct.WorkOrderId);
            if (workOrder.SourceRoomId != null)
            {
                await upsertProductStock(context: context,
                                   productId: workOrderProduct.ProductId,
                                   roomId: (int)workOrder.SourceRoomId,
                                   quantity: -1 * workOrderProduct.Quantity);
            }
            if (workOrder.TargetRoomId != null)
            {
                await upsertProductStock(context: context,
                                   productId: workOrderProduct.ProductId,
                                   roomId: (int)workOrder.TargetRoomId,
                                   quantity: workOrderProduct.Quantity);
            }
        }

        public Task workOrderUpdateProcuctStockChanges(InventoryDbContext context, WorkOrderProduct workOrderProduct)
        {
            throw new NotImplementedException();
        }

        public async Task workOrderDeleteProcuctStockChanges(InventoryDbContext context, params object[] keys)
        {
            throw new NotImplementedException();
        }
    }

    public interface IWorkOrderProductService : ICrudDataService<WorkOrderProduct>
    {
        Task<ItemsDTO<WorkOrderProduct>> GetsGridRowsAsync(Action<IGridColumnCollection<WorkOrderProduct>> columns, QueryDictionary<StringValues> query);
        Task workOrderInsertProcuctStockChanges(InventoryDbContext context, WorkOrderProduct workOrderProduct);
        Task workOrderUpdateProcuctStockChanges(InventoryDbContext context, WorkOrderProduct workOrderProduct);
        Task workOrderDeleteProcuctStockChanges(InventoryDbContext context, params object[] keys);
    }
}
