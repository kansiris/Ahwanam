namespace MaaAahwanam.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migrations_date_10062016 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EventInformations", "OrderDetailsid", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.EventInformations", "OrderDetailsid");
        }
    }
}
