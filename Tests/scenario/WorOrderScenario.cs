using System;
using dotnet_inventory_example.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System.Linq;
using GridShared;
using NLog;

internal class WorOrderScenario
{
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    private NorthwindDbContext context;

    public WorOrderScenario(NorthwindDbContext context)
    {
        this.context = context;
    }
    internal void InsertSourceRoomNull()
    {
        CustomersRepository repository = new CustomersRepository(context);
        var list = repository.GetAll()
            .Select(r => new SelectItem(r.CustomerID, r.CustomerID + " - " + r.CompanyName))
            .ToList();
        Console.WriteLine(list.Count);
    }
}