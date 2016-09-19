namespace MaaAahwanam.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migrations_date_09192016 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AdminTestimonials", "Orderid", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AdminTestimonials", "Orderid");
        }
    }
}
