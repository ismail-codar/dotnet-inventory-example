using System;
using dotnet_inventory_example.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System.Linq;
using GridShared;
using NLog;

namespace dotnet_inventory_example.Tests
{

    [TestClass]
    public class TestClass1
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private NorthwindDbContext context;

        [TestInitialize]
        public void Setup()
        {
            var builder = new DbContextOptionsBuilder<NorthwindDbContext>();
            string connectionString = "Server=localhost;Database=Northwind;Trusted_Connection=True;Integrated Security=false;User Id=sa;Password=codaricodar!%2300CODARyekbas";
            builder.UseSqlServer(connectionString);
            using (var context = new NorthwindDbContext(builder.Options))
            {
                CustomersRepository repository = new CustomersRepository(context);
                var list = repository.GetAll()
                    .Select(r => new SelectItem(r.CustomerID, r.CustomerID + " - " + r.CompanyName))
                    .ToList();
                Console.WriteLine(list.Count);
            }


            Log.Debug("test");
        }

        [TestMethod]
        public void Pass()
        {
            (1 + 1).ShouldBe(2);
        }


        [TestCleanup]
        public void Close()
        {
            Log.Debug("close");
        }
    }
}