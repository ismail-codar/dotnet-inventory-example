using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace dotnet_inventory_example.Models
{
    public partial class InventoryDbContext : DbContext
    {
        public InventoryDbContext()
        {
        }

        public InventoryDbContext(DbContextOptions<InventoryDbContext> options)
            : base(options)
        {
        }

        [DbFunction("RemoveDiacritics", "dbo")]
        public static string RemoveDiacritics(string input)
        {
            throw new NotImplementedException("This method can only be called in LINQ-to-Entities!");
        }

        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductStock> ProductStocks { get; set; }
        public virtual DbSet<ProductUnit> ProductUnits { get; set; }
        public virtual DbSet<StockBuilding> StockBuildings { get; set; }
        public virtual DbSet<StockRoom> StockRooms { get; set; }
        public virtual DbSet<WorkOrder> WorkOrders { get; set; }
        public virtual DbSet<WorkOrderProduct> WorkOrderProducts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=localhost;Database=inventory;Trusted_Connection=True;Integrated Security=false;User Id=sa;Password=codaricodar!%2300CODARyekbas");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Product");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ProductName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UnitPrice).HasColumnType("decimal(10, 2)");

                entity.HasOne(d => d.Unit)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.UnitId)
                    .HasConstraintName("Products2_FK");
            });

            modelBuilder.Entity<ProductStock>(entity =>
            {
                entity.HasKey(e => new { e.StockRoomId, e.ProductId })
                    .HasName("ProductStock_PK_StockRoom_Product");

                entity.ToTable("ProductStock");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductStocks)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("ProductStock_FK");

                entity.HasOne(d => d.StockRoom)
                    .WithMany(p => p.ProductStocks)
                    .HasForeignKey(d => d.StockRoomId)
                    .HasConstraintName("ProductStock_FK_1");
            });

            modelBuilder.Entity<ProductUnit>(entity =>
            {
                entity.HasKey(e => e.UnitId)
                    .HasName("Units_PK");

                entity.ToTable("ProductUnit");

                entity.Property(e => e.UnitName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<StockBuilding>(entity =>
            {
                entity.ToTable("StockBuilding");

                entity.Property(e => e.BuildingName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<StockRoom>(entity =>
            {
                entity.ToTable("StockRoom");

                entity.Property(e => e.RoomName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.StockBuilding)
                    .WithMany(p => p.StockRooms)
                    .HasForeignKey(d => d.StockBuildingId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("StockRoom_FK");
            });

            modelBuilder.Entity<WorkOrder>(entity =>
            {
                entity.ToTable("WorkOrder");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.HasOne(d => d.SourceRoom)
                    .WithMany(p => p.WorkOrderSourceRooms)
                    .HasForeignKey(d => d.SourceRoomId)
                    .HasConstraintName("WorkOrder_FK_SourceRoom");

                entity.HasOne(d => d.TargetRoom)
                    .WithMany(p => p.WorkOrderTargetRooms)
                    .HasForeignKey(d => d.TargetRoomId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("WorkOrder_FK_TargetRoom");
            });

            modelBuilder.Entity<WorkOrderProduct>(entity =>
            {
                entity.HasKey(e => new { e.WorkOrderId, e.ProductId })
                    .HasName("WorkOrderProducts_PK");

                entity.ToTable("WorkOrderProduct");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.WorkOrderProducts)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("WorkOrderProducts_FK_Product");

                entity.HasOne(d => d.WorkOrder)
                    .WithMany(p => p.WorkOrderProducts)
                    .HasForeignKey(d => d.WorkOrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("WorkOrderProducts_FK_WorkOrder");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
