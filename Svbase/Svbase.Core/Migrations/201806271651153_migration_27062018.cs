namespace Svbase.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migration_27062018 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Works",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Person", "WorkId", c => c.Int());
            CreateIndex("dbo.Person", "WorkId");
            AddForeignKey("dbo.Person", "WorkId", "dbo.Works", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Person", "WorkId", "dbo.Works");
            DropIndex("dbo.Person", new[] { "WorkId" });
            DropColumn("dbo.Person", "WorkId");
            DropTable("dbo.Works");
        }
    }
}
