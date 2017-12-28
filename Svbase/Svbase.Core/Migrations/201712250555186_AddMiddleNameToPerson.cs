namespace Svbase.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMiddleNameToPerson : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Person", "MiddleName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Person", "MiddleName");
        }
    }
}
