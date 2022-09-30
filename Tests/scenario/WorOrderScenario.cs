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
    internal async Task InsertSourceRoomNullAsync()
    {
        // CustomersRepository repository = new CustomersRepository(context);
        // var list = repository.GetAll()
        //     .Select(r => new SelectItem(r.CustomerID, r.CustomerID + " - " + r.CompanyName))
        //     .ToList();
        // Console.WriteLine(list.Count);

        WorkOrderService workOrderService = new WorkOrderService(this.options);
        await workOrderService.Insert(new WorkOrder()
        {
            Date = DateTime.Now,
            WorkOrderId = 0,
            ProductId = 0,
            SourceRoomId = null,
            TargetRoomId = 0
        });
    }
}