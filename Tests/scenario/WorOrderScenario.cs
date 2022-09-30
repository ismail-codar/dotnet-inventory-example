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
        // CustomersRepository repository = new CustomersRepository(context);
        // var list = repository.GetAll()
        //     .Select(r => new SelectItem(r.CustomerID, r.CustomerID + " - " + r.CompanyName))
        //     .ToList();
        // Console.WriteLine(list.Count);

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


        Log.Debug("Stock room oluştur");
        var stockRoom = new StockRoom()
        {
            StockBuildingId = stockBuilding.StockBuildingId,
            RoomName = "Oda 1"
        };
        await stockRoomService.Insert(stockRoom);

        // TODO workOrder & productStock

        // WorkOrderService workOrderService = new WorkOrderService(this.options);
        // await workOrderService.Insert(new WorkOrder()
        // {
        //     Date = DateTime.Now,
        //     WorkOrderId = 0,
        //     ProductId = 0,
        //     SourceRoomId = null,
        //     TargetRoomId = 0
        // });
    }
}