using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shop.Entities.ShopEntities;

namespace Shop.Persistence.EF.ShopRepositories.ProductProperties
{
    public class ProductPropertyEntityMap : IEntityTypeConfiguration<ProductProperty>
    {
        public void Configure(EntityTypeBuilder<ProductProperty> builder)
        {
            builder.ToTable("ProductProperties");
            builder.HasKey(_ => _.Id);

            builder.Property(_ => _.Id).IsRequired().ValueGeneratedOnAdd();
            builder.Property(_ => _.Key).IsRequired().HasMaxLength(150);
            builder.Property(_ => _.Value).IsRequired().HasMaxLength(1000);

            builder.HasOne(_ => _.Product)
                .WithMany(_ => _.Properties)
                .HasForeignKey(_ => _.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
