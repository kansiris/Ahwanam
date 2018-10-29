﻿using TrackerEnabledDbContext;
using System.Data.Entity;
using MaaAahwanam.Models;

namespace MaaAahwanam.Repository.db
{
    public class ApiContext : TrackerContext
    {
        public ApiContext()
            : base("name=APIContext")
        {
        }

        public DbSet<UserLogin> UserLogin { get; set; }
        public DbSet<UserDetail> UserDetail { get; set; }
        public DbSet<AdminTestimonial> AdminTesimonial { get; set; }
        public DbSet<AdminTestimonialPath> AdminTestimonialPath { get; set; }
        public DbSet<CartItem> CartItem { get; set; }
        public DbSet<Comment> Comment { get; set; }
        public DbSet<CommentDetail> CommentDetail { get; set; }
        public DbSet<Deal> Deal { get; set; }
        public DbSet<Enquiry> Enquiry { get; set; }
        public DbSet<EventDate> EventDate { get; set; }
        public DbSet<EventInformation> EventInformation { get; set; }
        public DbSet<EventsandTip> EventsandTip { get; set; }
        public DbSet<IssueDetail> IssueDetail { get; set; }
        public DbSet<IssueTicket> IssueTicket { get; set; }
        public DbSet<Payments_Requests> Payments_Requests { get; set; }
        public DbSet<Payment_Orders> Payment_Orders { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderDetail> OrderDetail { get; set; }
        public DbSet<OrdersServiceRequest> OrdersServiceRequest { get; set; }
        public DbSet<Review> Review { get; set; }
        public DbSet<ServiceRequest> ServiceRequest { get; set; }
        public DbSet<ServiceResponse> ServiceResponse { get; set; }
        public DbSet<SiteFaq> SiteFaq { get; set; }
        public DbSet<Subscription> Subscription { get; set; }
        public DbSet<UserAddBook> UserAddBook { get; set; }
        public DbSet<UserLogInTiming> UserLogInTiming { get; set; }
        public DbSet<VendorImage> VendorImage { get; set; }
        public DbSet<Vendormaster> Vendormaster { get; set; }
        public DbSet<VendorsBeautyService> VendorsBeautyService { get; set; }
        public DbSet<VendorsCatering> VendorsCatering { get; set; }
        public DbSet<VendorsDecorator> VendorsDecorator { get; set; }
        public DbSet<VendorsEntertainment> VendorsEntertainment { get; set; }
        public DbSet<VendorsEventOrganiser> VendorsEventOrganiser { get; set; }
        public DbSet<VendorsGift> VendorsGift { get; set; }
        public DbSet<VendorsInvitationCard> VendorsInvitationCard { get; set; }
        public DbSet<VendorsOther> VendorsOther { get; set; }
        public DbSet<VendorsPhotography> VendorsPhotography { get; set; }
        public DbSet<VendorsTravelandAccomodation> VendorsTravelandAccomodation { get; set; }
        public DbSet<VendorsWeddingCollection> VendorsWeddingCollection { get; set; }
        public DbSet<VendorVenue> VendorVenue { get; set; }
        public DbSet<TempVenueDetail> TempVenueDetail { get; set; }
        public DbSet<Availabledates> Availabledates { get; set; }
        public DbSet<Notification> Notification { get; set; }
        public DbSet<QuotationsList> QuotationsList { get; set; }
        public DbSet<VendorDates> VendorDates { get; set; }
        public DbSet<AvailableWhishLists> AvailableWhishLists { get; set; }
        public DbSet<NDeals> NDeal { get; set; }
        public DbSet<Package> Package { get; set; }
        public DbSet<OrderTransactionLog> OrderTransactionLog { get; set; }
        public DbSet<ContestMaster> ContestMaster { get; set; }
        public DbSet<Contest> Contest { get; set; }
        public DbSet<ContestVote> ContestVote { get; set; }
        public DbSet<QuoteResponse> QuoteResponse { get; set; }
        public DbSet<ManageUser> ManageUser { get; set; }
        public DbSet<ManageVendor> ManageVendor { get; set; }
        public DbSet<Policy> Policy { get; set; }
        public DbSet<Payment> Payment { get; set; }
        public DbSet<PackageMenu> PackageMenu { get; set; }
        public DbSet<Partner> Partner { get; set; }
        public DbSet<PartnerPackage> PartnerPackage { get; set; }
        public DbSet<PartnerContact> PartnerContact { get; set; }
        public DbSet<PartnerFile> PartnerFile { get; set; }
        public DbSet<SupplierServices> SupplierServices { get; set; }
    }
}
