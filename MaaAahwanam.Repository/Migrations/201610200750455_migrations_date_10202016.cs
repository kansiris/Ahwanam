namespace MaaAahwanam.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migrations_date_101820165 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ServiceRequests", "SubserviceType", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ServiceRequests", "SubserviceType");
        }
    }
}
