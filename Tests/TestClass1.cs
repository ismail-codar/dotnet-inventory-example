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
        WorOrderScenario worOrderScenario;

        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private NorthwindDbContext context;

        [TestInitialize]
        public void Setup()
        {
            var builder = new DbContextOptionsBuilder<NorthwindDbContext>();
            string connectionString = "Server=localhost;Database=Northwind;Trusted_Connection=True;Integrated Security=false;User Id=sa;Password=codaricodar!%2300CODARyekbas";
            builder.UseSqlServer(connectionString);
            this.context = new NorthwindDbContext(builder.Options);
            this.worOrderScenario = new WorOrderScenario(this.context);
        }

        [TestMethod]
        public void Pass()
        {
            // İçerdeki kaynak depo olmadan doğrudan dışarıdan depoya mal girişi
            worOrderScenario.InsertSourceRoomNull();
        }


        [TestCleanup]
        public void Close()
        {
            this.context.Dispose();
        }
    }
}