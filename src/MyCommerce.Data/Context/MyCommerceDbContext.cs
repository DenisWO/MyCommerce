using Microsoft.EntityFrameworkCore;
using MyCommerce.Business.Models;
using System.Linq;

namespace MyCommerce.Data.Context
{
    public class MyCommerceDbContext : DbContext
    {
        public MyCommerceDbContext(DbContextOptions<MyCommerceDbContext> options) : base(options) {}

        public DbSet<Product> Products { get; set; }
        public DbSet<Provider> Providers { get; set; }
        public DbSet<Address> Addresses { get; set; }

        //public DbSet<MyCommerce.App.ViewModels.ProviderViewModel> ProviderViewModel { get; set; }
        //public DbSet<MyCommerce.App.ViewModels.ProductViewModel> ProductViewModel { get; set; }
        //public DbSet<MyCommerce.App.ViewModels.AddressViewModel> AddressViewModel { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach(var property in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetProperties().Where(p => p.ClrType == typeof(string))))
                property.SetColumnType("varchar(100)");

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MyCommerceDbContext).Assembly);
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
                relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;

            base.OnModelCreating(modelBuilder);
        }
    }
}
