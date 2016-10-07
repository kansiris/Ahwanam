namespace MaaAahwanam.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migrations_date_100720162 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderDetails", "subid", c => c.Long(nullable: false));
            DropColumn("dbo.UserDetails", "subid");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UserDetails", "subid", c => c.Long(nullable: false));
            DropColumn("dbo.OrderDetails", "subid");
        }
    }
}
