namespace Svbase.Core.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class ChangeFlatConfiguration : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Table", newName: "Flat");
        }

        public override void Down()
        {
            RenameTable(name: "dbo.Flat", newName: "Table");
        }
    }
}
