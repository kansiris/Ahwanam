namespace MaaAahwanam.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migrations_date_082620162 : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.CartItems");
            AddColumn("dbo.EventInformations", "CartId", c => c.Long(nullable: false));
            AlterColumn("dbo.CartItems", "CartId", c => c.Long(nullable: false, identity: true));
            AddPrimaryKey("dbo.CartItems", "CartId");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.CartItems");
            AlterColumn("dbo.CartItems", "CartId", c => c.Long(nullable: false));
            DropColumn("dbo.EventInformations", "CartId");
            AddPrimaryKey("dbo.CartItems", "CartId");
        }
    }
}
