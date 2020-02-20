namespace Svbase.Core.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class ApartmentDistrictTable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.StreetDistrict", "StreetId", "dbo.Street");
            DropForeignKey("dbo.StreetDistrict", "DistrictId", "dbo.Districts");
            DropIndex("dbo.StreetDistrict", new[] { "StreetId" });
            DropIndex("dbo.StreetDistrict", new[] { "DistrictId" });
            CreateTable(
                "dbo.ApartmentDistrict",
                c => new
                {
                    ApartmentId = c.Int(nullable: false),
                    DistrictId = c.Int(nullable: false),
                })
                .PrimaryKey(t => new { t.ApartmentId, t.DistrictId })
                .ForeignKey("dbo.Apartment", t => t.ApartmentId, cascadeDelete: true)
                .ForeignKey("dbo.Districts", t => t.DistrictId, cascadeDelete: true)
                .Index(t => t.ApartmentId)
                .Index(t => t.DistrictId);

            DropTable("dbo.StreetDistrict");
        }

        public override void Down()
        {
            CreateTable(
                "dbo.StreetDistrict",
                c => new
                {
                    StreetId = c.Int(nullable: false),
                    DistrictId = c.Int(nullable: false),
                })
                .PrimaryKey(t => new { t.StreetId, t.DistrictId });

            DropForeignKey("dbo.ApartmentDistrict", "DistrictId", "dbo.Districts");
            DropForeignKey("dbo.ApartmentDistrict", "ApartmentId", "dbo.Apartment");
            DropIndex("dbo.ApartmentDistrict", new[] { "DistrictId" });
            DropIndex("dbo.ApartmentDistrict", new[] { "ApartmentId" });
            DropTable("dbo.ApartmentDistrict");
            CreateIndex("dbo.StreetDistrict", "DistrictId");
            CreateIndex("dbo.StreetDistrict", "StreetId");
            AddForeignKey("dbo.StreetDistrict", "DistrictId", "dbo.Districts", "Id", cascadeDelete: true);
            AddForeignKey("dbo.StreetDistrict", "StreetId", "dbo.Street", "Id", cascadeDelete: true);
        }
    }
}
