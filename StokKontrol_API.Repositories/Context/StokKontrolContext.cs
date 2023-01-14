using Microsoft.EntityFrameworkCore;
using StokKontrol_API.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StokKontrol_API.Repositories.Context
{
    public class StokKontrolContext : DbContext
    {
        //public StokKontrolContext(DbContextOptions<StokKontrolContext> options) : base(options)
        //{

        //}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Server=desktop-ufhr98h; Database=StokKontrolAPI; uid=sa; pwd=123");
        }

        private DbSet<Category> Categories { get; set; }
        private DbSet<Order> Orders { get; set; }
        private DbSet<OrderDetails> OrderDetails { get; set; }
        private DbSet<Product> Products { get; set; }
        private DbSet<Supplier> Suppliers { get; set; }
        private DbSet<User> Users { get; set; }
    }
}
