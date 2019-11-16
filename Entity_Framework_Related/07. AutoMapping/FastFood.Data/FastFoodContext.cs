﻿namespace FastFood.Data
{
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class FastFoodContext : DbContext
    {
        public FastFoodContext()
        {

        }

        public FastFoodContext(DbContextOptions<FastFoodContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<Item> Items { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderItem> OrderItems { get; set; }

        public DbSet<Position> Positions { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<OrderItem>()
                .HasKey(oi => new { oi.OrderId, oi.ItemId });


            builder.Entity<Position>()
                .HasKey(p => p.Id);
            builder.Entity<Item>()
                .HasKey(i => i.Id);

            //builder.Entity<Position>()
            //    .HasAlternateKey(p => p.Name);

            //builder.Entity<Item>()
            //    .HasAlternateKey(i => i.Name);
        }
    }
}
