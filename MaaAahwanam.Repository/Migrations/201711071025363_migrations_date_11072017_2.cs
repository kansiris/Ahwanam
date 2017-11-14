namespace MaaAahwanam.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migrations_date_11072017_2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.VendorImages", "ImageLimit", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.VendorImages", "ImageLimit");
        }
    }
}
