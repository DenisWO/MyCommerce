using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyCommerce.Business.Models;

namespace MyCommerce.Data.Mappings
{
    public class ProviderMapping : IEntityTypeConfiguration<Provider>
    {
        public void Configure(EntityTypeBuilder<Provider> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Name)
                .IsRequired()
                .HasColumnType("varchar(200)");
            builder.Property(p => p.Document)
                .IsRequired()
                .HasColumnType("varchar(14)");

            //Mapeamento Fornecedor/Endereço 1:1
            builder.HasOne(p => p.Address)
                .WithOne(a => a.Provider);

            //Mapeamento Fornecedor/Produtos 1:N
            builder.HasMany(p => p.Products)
                .WithOne(p => p.Provider)
                .HasForeignKey(p => p.ProviderId);

            builder.ToTable("Providers");
        }
    }
}
