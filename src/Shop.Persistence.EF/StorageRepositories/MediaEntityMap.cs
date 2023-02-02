using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shop.Entities.StorageEntities;

namespace Shop.Persistence.EF.StorageRepositories
{
    public class MediaEntityMap : IEntityTypeConfiguration<Media>
    {
        public void Configure(EntityTypeBuilder<Media> builder)
        {
            builder.ToTable("Medias");
            builder.HasKey(_ => _.Id);

            builder.Property(_ => _.Id).ValueGeneratedNever();
            builder.Property(_ => _.Data).IsRequired();
            builder.Property(_ => _.FileName).HasMaxLength(50).IsRequired();
            builder.Property(_ => _.Extension).HasMaxLength(10).IsRequired();
            builder.Property(_ => _.CreationDate).IsRequired();
        }
    }
}
