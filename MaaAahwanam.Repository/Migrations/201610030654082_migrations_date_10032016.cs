namespace MaaAahwanam.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migrations_date_10032016 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CartItems", "subid", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CartItems", "subid");
        }
    }
}
