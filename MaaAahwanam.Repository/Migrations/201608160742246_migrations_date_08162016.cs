namespace MaaAahwanam.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migrations_date_08162016 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Payment_Orders",
                c => new
                    {
                        PaymentID = c.Long(nullable: false, identity: true),
                        OrderID = c.Long(nullable: false),
                        paidamount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        cardnumber = c.String(),
                        CVV = c.String(),
                        Paiddate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.PaymentID);
            
            CreateTable(
                "dbo.Payments_Requests",
                c => new
                    {
                        PaymentID = c.Long(nullable: false, identity: true),
                        RequestID = c.Long(nullable: false),
                        paidamount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        cardnumber = c.String(),
                        CVV = c.String(),
                        Paiddate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.PaymentID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Payments_Requests");
            DropTable("dbo.Payment_Orders");
        }
    }
}
