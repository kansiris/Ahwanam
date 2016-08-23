namespace MaaAahwanam.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migrations_date_08232016 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CartItems", "attribute", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CartItems", "attribute");
        }
    }
}
