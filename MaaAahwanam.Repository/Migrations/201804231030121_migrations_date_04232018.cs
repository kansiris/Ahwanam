namespace MaaAahwanam.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migrations_date_04232018 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Packages",
                c => new
                    {
                        PackageID = c.Long(nullable: false, identity: true),
                        VendorId = c.Long(nullable: false),
                        VendorSubId = c.Long(nullable: false),
                        VendorType = c.String(),
                        VendorSubType = c.String(),
                        Category = c.String(),
                        PackageName = c.String(),
                        PackagePrice = c.String(),
                        PackageDescription = c.String(),
                        Status = c.String(),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.PackageID);
            
            AddColumn("dbo.NDeals", "UpdatedDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.NDeals", "UpdatedDate");
            DropTable("dbo.Packages");
        }
    }
}
