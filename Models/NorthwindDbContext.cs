using Microsoft.EntityFrameworkCore;
using System;

namespace dotnet_inventory_example.Models
{
    public class NorthwindDbContext : DbContext
    {

        public NorthwindDbContext(DbContextOptions<NorthwindDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderDetail>()
                .HasKey(c => new { c.OrderID, c.ProductID });

            modelBuilder.Entity<EmployeeTerritories>()
                .HasKey(r => new { r.EmployeeID, r.TerritoryID });

            modelBuilder.Entity<EmployeeTerritories>()
                .HasOne(r => r.Employee)
                .WithMany(s => s.Territories)
                .HasForeignKey(t => t.EmployeeID);

            modelBuilder.Entity<EmployeeTerritories>()
                .HasOne(r => r.Territory)
                .WithMany(s => s.Employees)
                .HasForeignKey(t => t.TerritoryID);

            base.OnModelCreating(modelBuilder);
        }

        [DbFunction("RemoveDiacritics", "dbo")]
        public static string RemoveDiacritics(string input)
        {
            throw new NotImplementedException("This method can only be called in LINQ-to-Entities!");
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Region> Region { get; set; }
        public DbSet<Shipper> Shippers { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Territory> Territories { get; set; }

        public DbSet<Product2> Products2 { get; set; }
        public DbSet<ProductUnit> ProductUnit { get; set; }
        public DbSet<ProductStock> ProductStocks { get; set; }
        public DbSet<StockBuilding> StockBuildings { get; set; }
        public DbSet<StockRoom> StockRooms { get; set; }
        public DbSet<WorkOrder> WorkOrders { get; set; }
    }
}