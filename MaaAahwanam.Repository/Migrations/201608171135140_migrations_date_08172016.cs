namespace MaaAahwanam.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migrations_date_08172016 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Notifications",
                c => new
                    {
                        id = c.Long(nullable: false, identity: true),
                        NotificationTo = c.Long(nullable: false),
                        type = c.String(),
                        description = c.String(),
                        DateandTime = c.DateTime(),
                        Subject = c.String(),
                        Status = c.String(),
                        EmailID = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Notifications");
        }
    }
}
