namespace Svbase.Core.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddPseudonymToStreet : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Street", "Pseudonym", c => c.String());
        }

        public override void Down()
        {
            DropColumn("dbo.Street", "Pseudonym");
        }
    }
}
