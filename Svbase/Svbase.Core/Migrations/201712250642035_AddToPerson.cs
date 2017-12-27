namespace Svbase.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddToPerson : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Person", "HomePhone", c => c.String());
            AddColumn("dbo.Person", "Email", c => c.String());
            AddColumn("dbo.Person", "PartionType", c => c.String());
            AlterColumn("dbo.Person", "Gender", c => c.Boolean(nullable: false));
            DropColumn("dbo.Person", "Patronymic");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Person", "Patronymic", c => c.String());
            AlterColumn("dbo.Person", "Gender", c => c.Int(nullable: false));
            DropColumn("dbo.Person", "PartionType");
            DropColumn("dbo.Person", "Email");
            DropColumn("dbo.Person", "HomePhone");
        }
    }
}
