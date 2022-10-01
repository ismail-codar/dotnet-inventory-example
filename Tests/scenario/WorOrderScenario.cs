using System;
using dotnet_inventory_example.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System.Linq;
using GridShared;
using NLog;
using dotnet_inventory_example.Services;
using System.Threading.Tasks;

internal class WorOrderScenario
{
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();
    DbContextOptions<NorthwindDbContext> options;

    public WorOrderScenario(DbContextOptions<NorthwindDbContext> options)
    {
        this.options = options;
    }
    internal async Task E2ETest()
    {
        IProductService2 productService = new Product2Service(this.options);
        IProductUnitService productUnitService = new ProductUnitService(this.options);
        IStockBuildingService stockBuildingService = new StockBuildingService(this.options);
        IStockRoomService stockRoomService = new StockRoomService(this.options);
        IWorkOrderService workOrderService = new WorkOrderService(this.options);
        IProductStockService productStockService = new ProductStockService(this.options);

        Log.Debug("Test ProductUnit (paket) oluştur");
        var productUnit = new ProductUnit()
        {
            UnitName = "Paket"
        };
        var stockBuilding = new StockBuilding()
        {
            BuildingName = "Bina 1",
        };
        try
        {
            await productUnitService.Insert(productUnit);
            Log.Debug("Test product oluştur");
            var product = new Product2()
            {
                Description = "deneme",
                ProductName = "Test product",
                UnitId = productUnit.UnitId,
                UnitPrice = 3,
                UnitsInStock = 6
            };
            await productService.Insert(product);

            Log.Debug("Stock building oluştur");
            await stockBuildingService.Insert(stockBuilding);


            Log.Debug("Stock room1 oluştur");
            var stockRoom1 = new StockRoom()
            {
                StockBuildingId = stockBuilding.StockBuildingId,
                RoomName = "Oda 1"
            };
            await stockRoomService.Insert(stockRoom1);

            Log.Debug("Stock room2 oluştur");
            var stockRoom2 = new StockRoom()
            {
                StockBuildingId = stockBuilding.StockBuildingId,
                RoomName = "Oda 2"
            };
            await stockRoomService.Insert(stockRoom2);

            // TODO workOrder & productStock
            Log.Debug("İçerdeki kaynak depo olmadan doğrudan dışarıdan depoya mal girişi");
            var workOrder1 = new WorkOrder()
            {
                Date = DateTime.Now,
                ProductId = product.ProductId,
                SourceRoomId = null,
                TargetRoomId = stockRoom1.StockRoomId,
                Quantity = 5
            };
            await workOrderService.Insert(workOrder1);
            Log.Debug(stockRoom1.RoomName + " de " + workOrder1.Quantity + " " + productUnit.UnitName + " " + product.ProductName + " olmalı");
            var productStock1 = productStockService.GetProductStockInRoom(product.ProductId, workOrder1.TargetRoomId);
            Log.Debug(productStock1);
            productStock1.Quantity.ShouldBe(workOrder1.Quantity);


            var workOrder2 = new WorkOrder()
            {
                Date = DateTime.Now,
                ProductId = product.ProductId,
                SourceRoomId = null,
                TargetRoomId = stockRoom1.StockRoomId,
                Quantity = 5
            };
            await workOrderService.Insert(workOrder2);
            Log.Debug(stockRoom1.RoomName + " de " + workOrder1.Quantity + workOrder2.Quantity + " " + productUnit.UnitName + " " + product.ProductName + " olmalı");
            var productStock2 = productStockService.GetProductStockInRoom(product.ProductId, workOrder2.TargetRoomId);
            Log.Debug(productStock2);
            productStock2.Quantity.ShouldBe(workOrder1.Quantity + workOrder2.Quantity);
        }
        catch (Exception ex)
        {
            Log.Error(ex);
            throw ex;
        }
        finally
        {
            await productUnitService.Delete(productUnit.UnitId);
            await stockBuildingService.Delete(stockBuilding.StockBuildingId);
        }
    }
}