using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shop.Entities.ShopEntities;

namespace Shop.Persistence.EF.ShopRepositories.Products
{
    public class ProductEntityMap : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");
            builder.HasKey(_ => _.Id);

            builder.Property(_ => _.Id).IsRequired().ValueGeneratedOnAdd();
            builder.Property(_ => _.Title).IsRequired();
            builder.Property(_ => _.Description).IsRequired();
            builder.Property(_ => _.Price).IsRequired();
            builder.Property(_ => _.Longitude).IsRequired();
            builder.Property(_ => _.Latitude).IsRequired();

            builder.OwnsOne(
            _ => _.Image,
            product =>
            {
                product.Property(media => media.Id)
                .HasColumnName("ImageId").IsRequired();
            });
        }
    }
}
