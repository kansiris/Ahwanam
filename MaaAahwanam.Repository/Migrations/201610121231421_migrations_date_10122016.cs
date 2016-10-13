namespace MaaAahwanam.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migrations_date_10122016 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reviews", "Sid", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Reviews", "Sid");
        }
    }
}
