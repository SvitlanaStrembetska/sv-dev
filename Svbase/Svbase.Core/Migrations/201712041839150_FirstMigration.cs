namespace Svbase.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FirstMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Apartment",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        StreetId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Street", t => t.StreetId, cascadeDelete: true)
                .Index(t => t.StreetId);
            
            CreateTable(
                "dbo.Table",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Number = c.String(),
                        ApartmentId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Apartment", t => t.ApartmentId, cascadeDelete: true)
                .Index(t => t.ApartmentId);
            
            CreateTable(
                "dbo.Person",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Patronymic = c.String(),
                        MobileTelephoneFirst = c.String(),
                        MobileTelephoneSecond = c.String(),
                        StationaryPhone = c.String(),
                        BirthdayDate = c.DateTime(nullable: false),
                        Position = c.String(),
                        Gender = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Beneficiaries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Street",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        CityId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cities", t => t.CityId, cascadeDelete: true)
                .Index(t => t.CityId);
            
            CreateTable(
                "dbo.Cities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Districts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        DistrictType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.PersonBeneficiary",
                c => new
                    {
                        PersonId = c.Int(nullable: false),
                        BeneficiaryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.PersonId, t.BeneficiaryId })
                .ForeignKey("dbo.Person", t => t.PersonId, cascadeDelete: true)
                .ForeignKey("dbo.Beneficiaries", t => t.BeneficiaryId, cascadeDelete: true)
                .Index(t => t.PersonId)
                .Index(t => t.BeneficiaryId);
            
            CreateTable(
                "dbo.PersonFlat",
                c => new
                    {
                        PersonId = c.Int(nullable: false),
                        FlatId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.PersonId, t.FlatId })
                .ForeignKey("dbo.Person", t => t.PersonId, cascadeDelete: true)
                .ForeignKey("dbo.Table", t => t.FlatId, cascadeDelete: true)
                .Index(t => t.PersonId)
                .Index(t => t.FlatId);
            
            CreateTable(
                "dbo.StreetDistrict",
                c => new
                    {
                        StreetId = c.Int(nullable: false),
                        DistrictId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.StreetId, t.DistrictId })
                .ForeignKey("dbo.Street", t => t.StreetId, cascadeDelete: true)
                .ForeignKey("dbo.Districts", t => t.DistrictId, cascadeDelete: true)
                .Index(t => t.StreetId)
                .Index(t => t.DistrictId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Apartment", "StreetId", "dbo.Street");
            DropForeignKey("dbo.StreetDistrict", "DistrictId", "dbo.Districts");
            DropForeignKey("dbo.StreetDistrict", "StreetId", "dbo.Street");
            DropForeignKey("dbo.Street", "CityId", "dbo.Cities");
            DropForeignKey("dbo.PersonFlat", "FlatId", "dbo.Table");
            DropForeignKey("dbo.PersonFlat", "PersonId", "dbo.Person");
            DropForeignKey("dbo.PersonBeneficiary", "BeneficiaryId", "dbo.Beneficiaries");
            DropForeignKey("dbo.PersonBeneficiary", "PersonId", "dbo.Person");
            DropForeignKey("dbo.Table", "ApartmentId", "dbo.Apartment");
            DropIndex("dbo.StreetDistrict", new[] { "DistrictId" });
            DropIndex("dbo.StreetDistrict", new[] { "StreetId" });
            DropIndex("dbo.PersonFlat", new[] { "FlatId" });
            DropIndex("dbo.PersonFlat", new[] { "PersonId" });
            DropIndex("dbo.PersonBeneficiary", new[] { "BeneficiaryId" });
            DropIndex("dbo.PersonBeneficiary", new[] { "PersonId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Street", new[] { "CityId" });
            DropIndex("dbo.Table", new[] { "ApartmentId" });
            DropIndex("dbo.Apartment", new[] { "StreetId" });
            DropTable("dbo.StreetDistrict");
            DropTable("dbo.PersonFlat");
            DropTable("dbo.PersonBeneficiary");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Districts");
            DropTable("dbo.Cities");
            DropTable("dbo.Street");
            DropTable("dbo.Beneficiaries");
            DropTable("dbo.Person");
            DropTable("dbo.Table");
            DropTable("dbo.Apartment");
        }
    }
}
