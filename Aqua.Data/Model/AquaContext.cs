using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Aqua.Data.Model
{
    public class AquaContext : DbContext
    {
        public AquaContext(DbContextOptions<AquaContext> options) 
            : base(options) 
        { }
        public virtual DbSet<AnimalEntity> Animals { get; set; }
        public virtual DbSet<CustomerEntity> Customers { get; set; }
        public virtual DbSet<InventoryItemEntity> Inventories { get; set; }
        public virtual DbSet< LocationEntity> Locations { get; set; }
        public virtual DbSet<OrderEntity> Orders { get; set; }
        public virtual DbSet<OrderItemEntity> OrderItems { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AnimalEntity>(entity =>
            {
                entity.ToTable("Animal", "Aqua");
                entity.Property(e => e.Name)
                    .IsRequired();
                entity.Property(e => e.Price)
                    .HasColumnType("smallmoney");
            });

            modelBuilder.Entity<CustomerEntity>(entity =>
            {
                entity.ToTable("Customer", "Aqua");
                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(100);
                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(100);
                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<InventoryItemEntity>(entity =>
            {
                entity.ToTable("InventoryItem", "Aqua");

                entity.HasOne(e => e.Animal)
                    .WithMany(e => e.Inventory)
                    .HasForeignKey(e => e.AnimalId)
                    .HasConstraintName("FK_InventoryItem_AnimalId");

                entity.HasOne(e => e.Location)
                    .WithMany(e => e.Inventory)
                    .HasForeignKey(e => e.LocationId)
                    .HasConstraintName("FK_InventoryItem_LocationId");

                entity.Property(e => e.Quantity)
                    .IsRequired()
                    .HasColumnType("int");

                entity.HasCheckConstraint(name: "CK_Inventory_Quantity_Nonnegative", sql: "[Quantity] >= 0");
            });

            modelBuilder.Entity<LocationEntity>(entity =>
            {
                entity.ToTable("Location", "Aqua");

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasColumnType("nvarchar(100)");
            });

            modelBuilder.Entity<OrderEntity>(entity =>
            {
                entity.ToTable("Order", "Aqua");

                entity.HasOne(e => e.Customer)
                    .WithMany(e => e.Orders)
                    .HasForeignKey(e => e.CustomerId)
                    .HasConstraintName("FK_Order_CustomerId");

                entity.HasOne(e => e.Location)
                    .WithMany(e => e.Orders)
                    .HasForeignKey(e => e.LocationId)
                    .HasConstraintName("FK_Order_LocationId");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime2")
                    .HasDefaultValueSql("(getdatetime())");

                entity.Property(e => e.Total)
                    .HasColumnType("smallmoney")
                    .IsRequired();

                entity.HasCheckConstraint(name: "CK_Order_Total_Nonnegative", sql: "[Total] >= 0");
            });

            modelBuilder.Entity<OrderItemEntity>(entity =>
            {
                entity.ToTable("OrderItem", "Aqua");

                entity.HasOne(e => e.Order)
                    .WithMany(e => e.OrderItems)
                    .HasForeignKey(e => e.OrderId)
                    .HasConstraintName("FK_OrderItem_OrderId");

                entity.HasOne(e => e.Animal)
                    .WithMany(e => e.OrderItems)
                    .HasForeignKey(e => e.AnimalId)
                    .HasConstraintName("FK_OrderItem_AnimalId");

                entity.Property(e => e.Quantity)
                    .IsRequired()
                    .HasColumnType("int");

                entity.Property(e => e.Total)
                    .HasColumnType("smallmoney")
                    .IsRequired();

                entity.HasCheckConstraint(name: "CK_OrderItem_Quantity_Nonnegative", sql: "[Quantity] >= 0");
                entity.HasCheckConstraint(name: "CK_OrderItem_Total_Nonnegative", sql: "[Total] >= 0");
            });
        }
    }
}
