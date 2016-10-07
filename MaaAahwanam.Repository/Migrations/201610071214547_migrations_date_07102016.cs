namespace MaaAahwanam.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migrations_date_07102016 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EventInformations", "subid", c => c.Long(nullable: false));
            AddColumn("dbo.OrderDetails", "subid", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrderDetails", "subid");
            DropColumn("dbo.EventInformations", "subid");
        }
    }
}
