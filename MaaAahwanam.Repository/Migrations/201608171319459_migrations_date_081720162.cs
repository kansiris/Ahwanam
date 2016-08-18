namespace MaaAahwanam.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migrations_date_081720162 : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.OrderDetails");
            AddColumn("dbo.OrderDetails", "OrderDetailId", c => c.Long(nullable: false, identity: true));
            AlterColumn("dbo.OrderDetails", "OrderId", c => c.Long(nullable: false));
            AddPrimaryKey("dbo.OrderDetails", "OrderDetailId");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.OrderDetails");
            AlterColumn("dbo.OrderDetails", "OrderId", c => c.Long(nullable: false, identity: true));
            DropColumn("dbo.OrderDetails", "OrderDetailId");
            AddPrimaryKey("dbo.OrderDetails", "OrderId");
        }
    }
}
