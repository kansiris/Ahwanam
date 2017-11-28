namespace MaaAahwanam.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migrations_date_11232017 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.VendorsBeautyServices", "discount", c => c.String());
            AddColumn("dbo.VendorsCaterings", "discount", c => c.String());
            AddColumn("dbo.VendorsDecorators", "discount", c => c.String());
            AddColumn("dbo.VendorsEntertainments", "discount", c => c.String());
            AddColumn("dbo.VendorsEventOrganisers", "discount", c => c.String());
            AddColumn("dbo.VendorsGifts", "discount", c => c.String());
            AddColumn("dbo.VendorsInvitationCards", "discount", c => c.String());
            AddColumn("dbo.VendorsOthers", "discount", c => c.String());
            AddColumn("dbo.VendorsPhotographies", "discount", c => c.String());
            AddColumn("dbo.VendorsTravelandAccomodations", "discount", c => c.String());
            AddColumn("dbo.VendorsWeddingCollections", "discount", c => c.String());
            AddColumn("dbo.VendorVenues", "discount", c => c.String());
            DropColumn("dbo.Vendormasters", "discount");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Vendormasters", "discount", c => c.String());
            DropColumn("dbo.VendorVenues", "discount");
            DropColumn("dbo.VendorsWeddingCollections", "discount");
            DropColumn("dbo.VendorsTravelandAccomodations", "discount");
            DropColumn("dbo.VendorsPhotographies", "discount");
            DropColumn("dbo.VendorsOthers", "discount");
            DropColumn("dbo.VendorsInvitationCards", "discount");
            DropColumn("dbo.VendorsGifts", "discount");
            DropColumn("dbo.VendorsEventOrganisers", "discount");
            DropColumn("dbo.VendorsEntertainments", "discount");
            DropColumn("dbo.VendorsDecorators", "discount");
            DropColumn("dbo.VendorsCaterings", "discount");
            DropColumn("dbo.VendorsBeautyServices", "discount");
        }
    }
}
