namespace Svbase.Core.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddPersonmigration : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Person", "BirthdayDate", c => c.DateTime());
        }

        public override void Down()
        {
            AlterColumn("dbo.Person", "BirthdayDate", c => c.DateTime(nullable: false));
        }
    }
}
