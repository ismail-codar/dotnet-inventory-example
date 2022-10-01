using Microsoft.EntityFrameworkCore;
using System;

namespace dotnet_inventory_example.Models
{
    public class InventoryDbContext : DbContext
    {

        public InventoryDbContext(DbContextOptions<InventoryDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductStock>()
                .HasKey(r => new { r.ProductId, r.StockRoomId });

            base.OnModelCreating(modelBuilder);
        }

        [DbFunction("RemoveDiacritics", "dbo")]
        public static string RemoveDiacritics(string input)
        {
            throw new NotImplementedException("This method can only be called in LINQ-to-Entities!");
        }


        public DbSet<Product> Product { get; set; }
        public DbSet<ProductUnit> ProductUnit { get; set; }
        public DbSet<ProductStock> ProductStock { get; set; }
        public DbSet<StockBuilding> StockBuilding { get; set; }
        public DbSet<StockRoom> StockRoom { get; set; }
        public DbSet<WorkOrder> WorkOrder { get; set; }
    }
}