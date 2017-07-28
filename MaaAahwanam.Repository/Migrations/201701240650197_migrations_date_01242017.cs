namespace MaaAahwanam.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migrations_date_01242017 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderDetails", "BookedDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrderDetails", "BookedDate");
        }
    }
}
