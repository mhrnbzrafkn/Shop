using FluentMigrator;
using System.Data;

namespace Shop.Migrations.Migrations
{
    [Migration(202302010004)]
    public class _202302010004_InitialProductPropertiesTable : Migration
    {
        public override void Up()
        {
            Create.Table("ProductProperties")
                .WithColumn("Id").AsString().NotNullable().PrimaryKey()
                .WithColumn("Key").AsString(150).NotNullable()
                .WithColumn("Value").AsString(1000).NotNullable()
                .WithColumn("ProductId").AsString().NotNullable()
                .ForeignKey("FK_ProductProperties_Products", "Products", "Id")
                .OnDelete(Rule.Cascade);
        }

        public override void Down()
        {
            Delete.Table("ProductProperties");
        }
    }
}
