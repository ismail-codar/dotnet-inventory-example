using System;
using dotnet_inventory_example.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System.Linq;
using GridShared;
using NLog;
using System.Threading.Tasks;

namespace dotnet_inventory_example.Tests
{
    [TestClass]
    public class TestClass1
    {
        WorOrderScenario worOrderScenario;

        private static readonly Logger Log = LogManager.GetCurrentClassLogger();


        [TestInitialize]
        public void Setup()
        {
            var builder = new DbContextOptionsBuilder<NorthwindDbContext>();
            string connectionString = "Server=localhost;Database=Northwind;Trusted_Connection=True;Integrated Security=false;User Id=sa;Password=codaricodar!%2300CODARyekbas";
            builder.UseSqlServer(connectionString);
            this.worOrderScenario = new WorOrderScenario(builder.Options);
        }

        [TestMethod]
        public async Task PassAsync()
        {
            Log.Debug("İçerdeki kaynak depo olmadan doğrudan dışarıdan depoya mal girişi");
            await worOrderScenario.E2ETest();
        }

    }
}