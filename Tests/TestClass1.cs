using System;
using dotnet_inventory_example.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace dotnet_inventory_example.Tests
{
    [TestClass]
    public class TestClass1
    {

        private NorthwindDbContext context;

        [TestInitialize]
        public void Setup()
        {
            var builder = new DbContextOptionsBuilder<NorthwindDbContext>();
            builder.UseSqlite();
            context = new NorthwindDbContext(builder.Options);
            Console.WriteLine("test");
        }

        [TestMethod]
        public void Pass()
        {
            (1 + 1).ShouldBe(2);
        }

        [TestMethod]
        public void AnotherPass()
        {
            (1 + 1).ShouldBe(2);

        }

        // [TestMethod()]
        // public void Fail()
        // {
        //     (1 + 1).ShouldBe(22);
        // }

        [TestMethod]
        [DataRow("First")]
        [DataRow("Second")]
        public void DataTest(string input)
        {
            (1 + 1).ShouldBe(2);
        }

    }
}