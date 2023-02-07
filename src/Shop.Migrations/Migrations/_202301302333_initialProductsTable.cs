using FluentMigrator;

namespace Shop.Migrations.Migrations
{
    [Migration(202301302333)]
    public class _202301302333_InitialProductsTable : Migration
    {
        public override void Up()
        {
            Create.Table("Products")
                .WithColumn("Id").AsString().NotNullable().PrimaryKey()
                .WithColumn("Title").AsString().NotNullable()
                .WithColumn("Description").AsString(1000).NotNullable()
                .WithColumn("Price").AsDouble().NotNullable()
                .WithColumn("ImageId").AsString().NotNullable()
                .WithColumn("Longitude").AsDouble().NotNullable()
                .WithColumn("Latitude").AsDouble().NotNullable()
                .WithColumn("CreationDate").AsDateTime().NotNullable();
        }

        public override void Down()
        {
            Delete.Table("Products");
        }
    }
}
