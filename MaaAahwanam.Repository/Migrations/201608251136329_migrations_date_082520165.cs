namespace MaaAahwanam.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migrations_date_082520165 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.ServiceRequests", "vendorbusinessname");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ServiceRequests", "vendorbusinessname", c => c.String());
        }
    }
}
