namespace MaaAahwanam.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migrations_date_10202016 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Deals", "DealServicePrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Deals", "DealServicePrice", c => c.Long(nullable: false));
        }
    }
}
