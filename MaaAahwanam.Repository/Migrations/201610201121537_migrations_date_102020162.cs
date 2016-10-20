namespace MaaAahwanam.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migrations_date_102020162 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CartItems", "Isdeal", c => c.Boolean(nullable: false));
            AddColumn("dbo.Deals", "VegPricePerPlate", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Deals", "NonVegPricePerPlate", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Deals", "BridalMakeupStartsFrom", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Deals", "PartyMakeupStartsFrom", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Deals", "CardCost", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Deals", "CardCostWithPrint", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.OrderDetails", "Isdeal", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrderDetails", "Isdeal");
            DropColumn("dbo.Deals", "CardCostWithPrint");
            DropColumn("dbo.Deals", "CardCost");
            DropColumn("dbo.Deals", "PartyMakeupStartsFrom");
            DropColumn("dbo.Deals", "BridalMakeupStartsFrom");
            DropColumn("dbo.Deals", "NonVegPricePerPlate");
            DropColumn("dbo.Deals", "VegPricePerPlate");
            DropColumn("dbo.CartItems", "Isdeal");
        }
    }
}
