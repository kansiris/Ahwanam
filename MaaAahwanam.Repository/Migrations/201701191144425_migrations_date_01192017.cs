namespace MaaAahwanam.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migrations_date_01192017 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Availabledates", "vendorsubid", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Availabledates", "vendorsubid");
        }
    }
}
