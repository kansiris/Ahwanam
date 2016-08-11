namespace MaaAahwanam.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migrations_date_08102016 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AdminTestimonials",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        Email = c.String(),
                        UpdatedBy = c.Long(nullable: false),
                        UpdatedDate = c.DateTime(),
                        Status = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AdminTestimonialPaths",
                c => new
                    {
                        PathId = c.Long(nullable: false, identity: true),
                        Id = c.Long(nullable: false),
                        ImagePath = c.String(),
                        VideoPath = c.String(),
                        UpdatedBy = c.Long(nullable: false),
                        UpdatedDate = c.DateTime(),
                        Status = c.String(),
                    })
                .PrimaryKey(t => t.PathId);
            
            CreateTable(
                "dbo.AuditLogs",
                c => new
                    {
                        AuditLogId = c.Long(nullable: false, identity: true),
                        UserName = c.String(),
                        EventDateUTC = c.DateTime(nullable: false),
                        EventType = c.Int(nullable: false),
                        TypeFullName = c.String(nullable: false, maxLength: 512),
                        RecordId = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.AuditLogId);
            
            CreateTable(
                "dbo.AuditLogDetails",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        PropertyName = c.String(nullable: false, maxLength: 256),
                        OriginalValue = c.String(),
                        NewValue = c.String(),
                        AuditLogId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AuditLogs", t => t.AuditLogId, cascadeDelete: true)
                .Index(t => t.AuditLogId);
            
            CreateTable(
                "dbo.Availabledates",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        vendorId = c.Long(nullable: false),
                        servicetype = c.String(),
                        availabledate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CartItems",
                c => new
                    {
                        CartId = c.Long(nullable: false, identity: true),
                        VendorId = c.Long(nullable: false),
                        ServiceType = c.String(),
                        ServicePrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Perunitprice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Quantity = c.Int(nullable: false),
                        Discount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DiscountPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Orderedby = c.Long(nullable: false),
                        Status = c.String(),
                        UpdatedBy = c.Long(nullable: false),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.CartId);
            
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        CommentId = c.Long(nullable: false, identity: true),
                        ServiceId = c.String(),
                        ServiceType = c.String(),
                        Status = c.String(),
                        UpdatedBy = c.Long(nullable: false),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.CommentId);
            
            CreateTable(
                "dbo.CommentDetails",
                c => new
                    {
                        CommentDetId = c.Long(nullable: false, identity: true),
                        CommentId = c.Long(nullable: false),
                        UserLoginId = c.Int(nullable: false),
                        CommentDetails = c.String(),
                        CommentDate = c.DateTime(),
                        AttachFileName = c.String(),
                        UpdatedBy = c.Long(nullable: false),
                        UpdatedDate = c.DateTime(),
                        Status = c.String(),
                    })
                .PrimaryKey(t => t.CommentDetId);
            
            CreateTable(
                "dbo.Enquiries",
                c => new
                    {
                        EnquiryId = c.Long(nullable: false, identity: true),
                        PersonName = c.String(),
                        SenderEmailId = c.String(),
                        SenderPhone = c.String(),
                        EnquiryTitle = c.String(),
                        EnquiryDetails = c.String(),
                        EnquiryDate = c.DateTime(),
                        EnquiryStatus = c.String(),
                        Country = c.String(),
                        PostalCode = c.String(),
                        CompanyName = c.String(),
                        UpdatedBy = c.Long(nullable: false),
                        UpdatedDate = c.DateTime(),
                        Status = c.String(),
                    })
                .PrimaryKey(t => t.EnquiryId);
            
            CreateTable(
                "dbo.EventDates",
                c => new
                    {
                        EventDateId = c.Long(nullable: false, identity: true),
                        EventId = c.Long(nullable: false),
                        StartDate = c.DateTime(),
                        StartTime = c.String(),
                        EndDate = c.DateTime(),
                        EndTime = c.String(),
                        UpdatedBy = c.Long(nullable: false),
                        UpdatedDate = c.DateTime(),
                        Status = c.String(),
                    })
                .PrimaryKey(t => t.EventDateId);
            
            CreateTable(
                "dbo.EventInformations",
                c => new
                    {
                        EventId = c.String(nullable: false, maxLength: 128),
                        OrderId = c.Long(nullable: false),
                        EventName = c.String(),
                        Address = c.String(),
                        Location = c.String(),
                        State = c.String(),
                        City = c.String(),
                        PostalCode = c.String(),
                        Phone = c.String(),
                        Email = c.String(),
                        UpdatedBy = c.Long(nullable: false),
                        UpdatedDate = c.DateTime(),
                        Status = c.String(),
                    })
                .PrimaryKey(t => t.EventId);
            
            CreateTable(
                "dbo.EventsandTips",
                c => new
                    {
                        EventId = c.Long(nullable: false, identity: true),
                        Title = c.String(),
                        Description = c.String(),
                        Location = c.String(),
                        Image = c.String(),
                        Phone = c.String(),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Person = c.String(),
                        Type = c.String(),
                        Link = c.String(),
                        Eventstartdate = c.DateTime(),
                        Eventenddate = c.DateTime(),
                        Imageid = c.String(),
                        UpdatedBy = c.Long(nullable: false),
                        UpdatedDate = c.DateTime(nullable: false),
                        Status = c.String(),
                    })
                .PrimaryKey(t => t.EventId);
            
            CreateTable(
                "dbo.IssueDetails",
                c => new
                    {
                        TicketCommuId = c.Long(nullable: false, identity: true),
                        TicketId = c.Long(nullable: false),
                        Msg = c.String(),
                        RepliedBy = c.Long(nullable: false),
                        ReplayedDate = c.DateTime(),
                        UpdatedBy = c.Long(nullable: false),
                        UpdatedDate = c.DateTime(),
                        Status = c.String(),
                    })
                .PrimaryKey(t => t.TicketCommuId);
            
            CreateTable(
                "dbo.IssueTickets",
                c => new
                    {
                        TicketId = c.Long(nullable: false, identity: true),
                        UserLoginId = c.Long(nullable: false),
                        OrderId = c.Long(nullable: false),
                        IssueType = c.String(),
                        Subject = c.String(),
                        Severity = c.String(),
                        Name = c.String(),
                        Status = c.String(),
                        ClosedDate = c.DateTime(),
                        UpdatedBy = c.Long(nullable: false),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.TicketId);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        OrderId = c.Long(nullable: false, identity: true),
                        OrderedBy = c.Long(nullable: false),
                        OrderNumber = c.Long(nullable: false),
                        PaymentId = c.Long(nullable: false),
                        OrderDate = c.DateTime(),
                        TotalPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        UpdatedBy = c.Long(nullable: false),
                        UpdatedDate = c.DateTime(),
                        Status = c.String(),
                    })
                .PrimaryKey(t => t.OrderId);
            
            CreateTable(
                "dbo.OrderDetails",
                c => new
                    {
                        OrderId = c.Long(nullable: false, identity: true),
                        VendorId = c.Long(nullable: false),
                        ServiceType = c.String(),
                        ServicePrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PerunitPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Quantity = c.Int(nullable: false),
                        Discount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DiscountPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        OrderBy = c.Long(nullable: false),
                        PaymentId = c.Long(nullable: false),
                        Status = c.String(),
                        UpdatedBy = c.Long(nullable: false),
                        UpdatedDate = c.DateTime(),
                        OrderNo = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.OrderId);
            
            CreateTable(
                "dbo.OrdersServiceRequests",
                c => new
                    {
                        OrderId = c.Long(nullable: false, identity: true),
                        ResponseId = c.Long(nullable: false),
                        TotalPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PaymentId = c.Long(nullable: false),
                        ServiceType = c.String(),
                        OrderNumber = c.Long(nullable: false),
                        UpdatedBy = c.Long(nullable: false),
                        UpdatedDate = c.DateTime(),
                        Status = c.String(),
                    })
                .PrimaryKey(t => t.OrderId);
            
            CreateTable(
                "dbo.Reviews",
                c => new
                    {
                        ReviewId = c.Long(nullable: false, identity: true),
                        FirstName = c.String(),
                        EmailId = c.String(),
                        Service = c.String(),
                        ServiceId = c.Long(nullable: false),
                        Comments = c.String(),
                        UpdatedBy = c.Int(nullable: false),
                        UpdatedDate = c.DateTime(),
                        Status = c.String(),
                    })
                .PrimaryKey(t => t.ReviewId);
            
            CreateTable(
                "dbo.ServiceRequests",
                c => new
                    {
                        RequestId = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        EmailId = c.String(),
                        Budget = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ServiceType = c.String(),
                        Preferences = c.String(),
                        EventName = c.String(),
                        EventAddress = c.String(),
                        EventLocation = c.String(),
                        State = c.String(),
                        City = c.String(),
                        PostalCode = c.String(),
                        PhoneNo = c.String(),
                        EventStartDate = c.DateTime(),
                        EventStartTime = c.DateTime(),
                        EventEnddate = c.DateTime(),
                        EventEndtime = c.DateTime(),
                        VendorId = c.Long(nullable: false),
                        UpdatedBy = c.Long(nullable: false),
                        UpdatedTime = c.DateTime(),
                        Status = c.String(),
                        Type = c.String(),
                    })
                .PrimaryKey(t => t.RequestId);
            
            CreateTable(
                "dbo.ServiceResponses",
                c => new
                    {
                        ResponseId = c.Long(nullable: false, identity: true),
                        RequestId = c.Long(nullable: false),
                        Description = c.String(),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ResponseBy = c.Long(nullable: false),
                        UpdatedBy = c.Long(nullable: false),
                        UpdatedDate = c.DateTime(),
                        Status = c.String(),
                        VendorType = c.String(),
                    })
                .PrimaryKey(t => t.ResponseId);
            
            CreateTable(
                "dbo.SiteFaqs",
                c => new
                    {
                        FaqId = c.Long(nullable: false, identity: true),
                        Category = c.String(),
                        Question = c.String(),
                        Answer = c.String(),
                        UpdatedBy = c.Long(nullable: false),
                        UpdatedDate = c.DateTime(),
                        Status = c.String(),
                    })
                .PrimaryKey(t => t.FaqId);
            
            CreateTable(
                "dbo.Subscriptions",
                c => new
                    {
                        SubscriptionId = c.Long(nullable: false, identity: true),
                        EmailId = c.String(),
                        Status = c.String(),
                        UpdatedBy = c.Long(),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.SubscriptionId);
            
            CreateTable(
                "dbo.TempVenueDetails",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        VenueType = c.String(),
                        BestSuited = c.String(),
                        VenueConfig = c.String(),
                        ContactPerson = c.String(),
                        Address = c.String(),
                        Localities = c.String(),
                        VenueArea = c.String(),
                        DiningArea = c.String(),
                        IndoorSpace = c.String(),
                        OutdoorSpace = c.String(),
                        RoomsAvailable = c.String(),
                        Numofrooms = c.String(),
                        EstablishedDate = c.String(),
                        EventDescription = c.String(),
                        EmailID = c.String(),
                        BookingAdvanceAmount = c.Decimal(precision: 18, scale: 2),
                        BookingTotalAmount = c.Decimal(precision: 18, scale: 2),
                        PaymentMode = c.String(),
                        ManagerAvl = c.String(),
                        ManagerContactInfo = c.String(),
                        Images = c.String(),
                        TwoWheelerPlace = c.String(),
                        FourWheelerPlace = c.String(),
                        CateringContactInfo = c.String(),
                        FoodType = c.String(),
                        CateringIndoorCap = c.String(),
                        CateringOutdoorCap = c.String(),
                        Alchol = c.String(),
                        Wifi = c.String(),
                        Tables = c.String(),
                        Stage = c.String(),
                        Washrooms = c.String(),
                        Openairspace = c.String(),
                        Dj = c.String(),
                        Elevator = c.String(),
                        DecoratorContactInfo = c.String(),
                        DecorationType = c.String(),
                        PhotographerContactInfo = c.String(),
                        Videographercontactinfo = c.String(),
                        LCDProjector = c.String(),
                        Overheadprojector = c.String(),
                        FullAudioandVideo = c.String(),
                        Builtinscreens = c.String(),
                        MenBoutiqueContactInfo = c.String(),
                        WomenBoutiqueContactInfo = c.String(),
                        Halltype = c.String(),
                        Venuename = c.String(),
                        CancelPolicy = c.String(),
                        AvgEvents = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.UserAddBooks",
                c => new
                    {
                        AddBookId = c.Long(nullable: false, identity: true),
                        UserLoginId = c.Long(nullable: false),
                        PersonName = c.String(),
                        PersonAddress = c.String(),
                        PersonLocation = c.String(),
                        PersonCity = c.String(),
                        PersonPhone = c.String(),
                        PersonEmail = c.String(),
                        UpdatedBy = c.Long(nullable: false),
                        UpdatedDate = c.DateTime(),
                        Status = c.String(),
                    })
                .PrimaryKey(t => t.AddBookId);
            
            CreateTable(
                "dbo.UserDetails",
                c => new
                    {
                        UserDetailId = c.Long(nullable: false, identity: true),
                        UserLoginId = c.Long(nullable: false),
                        FirstName = c.String(),
                        LastName = c.String(),
                        AlternativeEmailID = c.String(),
                        Country = c.String(),
                        Gender = c.String(),
                        Landmark = c.String(),
                        UserPhone = c.String(),
                        Url = c.String(),
                        Address = c.String(),
                        City = c.String(),
                        State = c.String(),
                        ZipCode = c.String(),
                        UserImgId = c.String(),
                        UserImgName = c.String(),
                        Status = c.String(),
                        UpdatedBy = c.Long(nullable: false),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.UserDetailId);
            
            CreateTable(
                "dbo.UserLogins",
                c => new
                    {
                        UserLoginId = c.Long(nullable: false, identity: true),
                        UserName = c.String(),
                        Password = c.String(),
                        UserType = c.String(),
                        RegDate = c.DateTime(),
                        Status = c.String(),
                        UpdatedBy = c.Long(nullable: false),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.UserLoginId);
            
            CreateTable(
                "dbo.UserLogInTimings",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        Type = c.String(),
                        Date = c.DateTime(),
                        Status = c.String(),
                        UpdatedBy = c.Long(nullable: false),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.VendorImages",
                c => new
                    {
                        ImageId = c.Long(nullable: false, identity: true),
                        VendorId = c.Long(nullable: false),
                        VendorMasterId = c.Long(nullable: false),
                        ImageName = c.String(),
                        UpdatedBy = c.Long(nullable: false),
                        UpdatedDate = c.DateTime(),
                        Status = c.String(),
                    })
                .PrimaryKey(t => t.ImageId);
            
            CreateTable(
                "dbo.Vendormasters",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserId = c.Long(nullable: false),
                        BusinessName = c.String(),
                        Address = c.String(),
                        Landmark = c.String(),
                        City = c.String(),
                        State = c.String(),
                        ZipCode = c.String(),
                        Description = c.String(),
                        ContactPerson = c.String(),
                        ContactNumber = c.String(),
                        LandlineNumber = c.String(),
                        EmailId = c.String(),
                        Url = c.String(),
                        Experience = c.String(),
                        EstablishedYear = c.String(),
                        ServicType = c.String(),
                        Status = c.String(),
                        UpdatedBy = c.Long(nullable: false),
                        UpdatedDate = c.DateTime(),
                        Quotation = c.Boolean(nullable: false),
                        Bidding = c.Boolean(nullable: false),
                        ReverseBidding = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.VendorsBeautyServices",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        VendorMasterId = c.Long(nullable: false),
                        Category = c.String(),
                        BridalMakeupStartsFrom = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PartyMakeupStartsFrom = c.Decimal(nullable: false, precision: 18, scale: 2),
                        HomeServices = c.String(),
                        Status = c.String(),
                        UpdatedBy = c.Long(nullable: false),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.VendorsCaterings",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        VendorMasterId = c.Long(nullable: false),
                        CuisineType = c.String(),
                        Veg = c.Decimal(nullable: false, precision: 18, scale: 2),
                        NonVeg = c.Decimal(nullable: false, precision: 18, scale: 2),
                        MinOrder = c.String(),
                        MaxOrder = c.String(),
                        MineralWaterIncluded = c.String(),
                        TransportIncluded = c.String(),
                        Menuitems = c.String(),
                        LiveCookingStation = c.String(),
                        Status = c.String(),
                        UpdatedBy = c.Long(nullable: false),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.VendorsDecorators",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        VendorMasterId = c.Long(nullable: false),
                        DecorationType = c.String(),
                        StartingPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Status = c.String(),
                        UpdatedBy = c.Long(nullable: false),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.VendorsEntertainments",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        VendorMasterId = c.Long(nullable: false),
                        Category = c.String(),
                        StartingPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Status = c.String(),
                        UpdatedBy = c.Long(nullable: false),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.VendorsEventOrganisers",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        VendorMasterId = c.Long(nullable: false),
                        StartingPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Status = c.String(),
                        UpdatedBy = c.Long(nullable: false),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.VendorsGifts",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        VendorMasterId = c.Long(nullable: false),
                        GiftType = c.String(),
                        MinOrder = c.String(),
                        MaxOrder = c.String(),
                        GiftCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Status = c.String(),
                        UpdatedBy = c.Long(nullable: false),
                        UpdatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.VendorsInvitationCards",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        VendorMasterId = c.Long(nullable: false),
                        CardType = c.String(),
                        DesignName = c.String(),
                        CardCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CardCostWithPrint = c.Decimal(nullable: false, precision: 18, scale: 2),
                        MinOrder = c.String(),
                        Status = c.String(),
                        UpdatedBy = c.Long(nullable: false),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.VendorsOthers",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        VendorMasterId = c.Long(nullable: false),
                        ItemCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        MinOrder = c.Long(nullable: false),
                        MaxOrder = c.Long(nullable: false),
                        Status = c.String(),
                        UpdatedBy = c.Long(nullable: false),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.VendorsPhotographies",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        VendorMasterId = c.Long(nullable: false),
                        PhotographyType = c.String(),
                        PreWeddingShoot = c.String(),
                        DestinationPhotography = c.String(),
                        StartingPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PriorBookingsDays = c.String(),
                        Status = c.String(),
                        UpdatedBy = c.Long(nullable: false),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.VendorsTravelandAccomodations",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        VendorMasterId = c.Long(),
                        Category = c.String(),
                        Startsfrom = c.Decimal(precision: 18, scale: 2),
                        Status = c.String(),
                        UpdatedBy = c.Long(),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.VendorsWeddingCollections",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        VendorMasterId = c.Long(nullable: false),
                        OnlineStore = c.String(),
                        InStore = c.String(),
                        Status = c.String(),
                        UpdatedBy = c.Long(nullable: false),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.VendorVenues",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        VendorMasterId = c.Long(nullable: false),
                        VenueType = c.String(),
                        Food = c.String(),
                        CockTails = c.String(),
                        Rooms = c.String(),
                        SeatingCapacity = c.Int(nullable: false),
                        DiningCapacity = c.Int(nullable: false),
                        Minimumseatingcapacity = c.Int(nullable: false),
                        Maximumcapacity = c.Int(nullable: false),
                        VegLunchCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        NonVegLunchCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        VegDinnerCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        NonVegDinnerCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        MinOrder = c.String(),
                        MaxOrder = c.String(),
                        DecorationAllowed = c.String(),
                        HallType = c.String(),
                        Wifi = c.String(),
                        Menuwiththenoofitems = c.String(),
                        Distancefrommainplaceslike = c.String(),
                        LiveCookingStation = c.String(),
                        ServiceCost = c.Long(nullable: false),
                        Status = c.String(),
                        UpdatedBy = c.Long(nullable: false),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AuditLogDetails", "AuditLogId", "dbo.AuditLogs");
            DropIndex("dbo.AuditLogDetails", new[] { "AuditLogId" });
            DropTable("dbo.VendorVenues");
            DropTable("dbo.VendorsWeddingCollections");
            DropTable("dbo.VendorsTravelandAccomodations");
            DropTable("dbo.VendorsPhotographies");
            DropTable("dbo.VendorsOthers");
            DropTable("dbo.VendorsInvitationCards");
            DropTable("dbo.VendorsGifts");
            DropTable("dbo.VendorsEventOrganisers");
            DropTable("dbo.VendorsEntertainments");
            DropTable("dbo.VendorsDecorators");
            DropTable("dbo.VendorsCaterings");
            DropTable("dbo.VendorsBeautyServices");
            DropTable("dbo.Vendormasters");
            DropTable("dbo.VendorImages");
            DropTable("dbo.UserLogInTimings");
            DropTable("dbo.UserLogins");
            DropTable("dbo.UserDetails");
            DropTable("dbo.UserAddBooks");
            DropTable("dbo.TempVenueDetails");
            DropTable("dbo.Subscriptions");
            DropTable("dbo.SiteFaqs");
            DropTable("dbo.ServiceResponses");
            DropTable("dbo.ServiceRequests");
            DropTable("dbo.Reviews");
            DropTable("dbo.OrdersServiceRequests");
            DropTable("dbo.OrderDetails");
            DropTable("dbo.Orders");
            DropTable("dbo.IssueTickets");
            DropTable("dbo.IssueDetails");
            DropTable("dbo.EventsandTips");
            DropTable("dbo.EventInformations");
            DropTable("dbo.EventDates");
            DropTable("dbo.Enquiries");
            DropTable("dbo.CommentDetails");
            DropTable("dbo.Comments");
            DropTable("dbo.CartItems");
            DropTable("dbo.Availabledates");
            DropTable("dbo.AuditLogDetails");
            DropTable("dbo.AuditLogs");
            DropTable("dbo.AdminTestimonialPaths");
            DropTable("dbo.AdminTestimonials");
        }
    }
}
