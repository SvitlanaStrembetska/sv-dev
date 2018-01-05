namespace Svbase.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPersonFilemigration : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Person", "HomePhone");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Person", "HomePhone", c => c.String());
        }
    }
}
