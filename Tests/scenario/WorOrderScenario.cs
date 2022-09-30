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
        IProductStockService productStockService = new ProductStockService(this.options);
        IWorkOrderService workOrderService = new WorkOrderService(this.options);

        Log.Debug("Test ProductUnit (paket) oluştur");
        var productUnit = new ProductUnit()
        {
            UnitName = "Paket"
        };
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
        var stockBuilding = new StockBuilding()
        {
            BuildingName = "Bina 1",
        };
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
        await workOrderService.Insert(new WorkOrder()
        {
            Date = DateTime.Now,
            ProductId = product.ProductId,
            SourceRoomId = null,
            TargetRoomId = stockRoom1.StockRoomId
        });
    }
}