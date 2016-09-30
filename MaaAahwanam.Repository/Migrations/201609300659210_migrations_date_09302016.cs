namespace MaaAahwanam.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migrations_date_09302016 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ServiceRequests", "EventStartDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.ServiceRequests", "EventStartTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.ServiceRequests", "EventEnddate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.ServiceRequests", "EventEndtime", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ServiceRequests", "EventEndtime", c => c.DateTime());
            AlterColumn("dbo.ServiceRequests", "EventEnddate", c => c.DateTime());
            AlterColumn("dbo.ServiceRequests", "EventStartTime", c => c.DateTime());
            AlterColumn("dbo.ServiceRequests", "EventStartDate", c => c.DateTime());
        }
    }
}
