using FluentMigrator;

namespace Shop.Migrations.Migrations
{
    [Migration(202302021208)]
    public class _202302021208_initialMediasTable : Migration
    {
        public override void Up()
        {
            Create.Table("Medias")
                .WithColumn("Id").AsString().NotNullable().PrimaryKey()
                .WithColumn("Data").AsBinary(int.MaxValue)
                .WithColumn("FileName").AsString(50).Nullable()
                .WithColumn("Extension").AsString(10).Nullable()
                .WithColumn("CreationDate").AsDateTime2().NotNullable();
        }

        public override void Down()
        {
            Delete.Table("Medias");
        }
    }
}
