namespace MaaAahwanam.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migrations_date_08112016 : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.EventInformations");
            AlterColumn("dbo.EventInformations", "EventId", c => c.Long(nullable: false, identity: true));
            AddPrimaryKey("dbo.EventInformations", "EventId");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.EventInformations");
            AlterColumn("dbo.EventInformations", "EventId", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.EventInformations", "EventId");
        }
    }
}
