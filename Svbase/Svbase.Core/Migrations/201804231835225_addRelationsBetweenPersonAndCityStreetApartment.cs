namespace Svbase.Core.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class addRelationsBetweenPersonAndCityStreetApartment : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Person", "CityId", c => c.Int());
            AddColumn("dbo.Person", "StreetId", c => c.Int());
            AddColumn("dbo.Person", "ApartmentId", c => c.Int());
            CreateIndex("dbo.Person", "CityId");
            CreateIndex("dbo.Person", "StreetId");
            CreateIndex("dbo.Person", "ApartmentId");
            AddForeignKey("dbo.Person", "ApartmentId", "dbo.Apartment", "Id");
            AddForeignKey("dbo.Person", "CityId", "dbo.Cities", "Id");
            AddForeignKey("dbo.Person", "StreetId", "dbo.Street", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.Person", "StreetId", "dbo.Street");
            DropForeignKey("dbo.Person", "CityId", "dbo.Cities");
            DropForeignKey("dbo.Person", "ApartmentId", "dbo.Apartment");
            DropIndex("dbo.Person", new[] { "ApartmentId" });
            DropIndex("dbo.Person", new[] { "StreetId" });
            DropIndex("dbo.Person", new[] { "CityId" });
            DropColumn("dbo.Person", "ApartmentId");
            DropColumn("dbo.Person", "StreetId");
            DropColumn("dbo.Person", "CityId");
        }
    }
}
