namespace MaaAahwanam.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migrations_date_09272016 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.VendorsOthers", "MinOrder", c => c.String());
            AlterColumn("dbo.VendorsOthers", "MaxOrder", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.VendorsOthers", "MaxOrder", c => c.Long(nullable: false));
            AlterColumn("dbo.VendorsOthers", "MinOrder", c => c.Long(nullable: false));
        }
    }
}
