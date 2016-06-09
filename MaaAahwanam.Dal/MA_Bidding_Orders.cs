//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MaaAahwanam.Dal
{
    using System;
    using System.Collections.Generic;
    
    public partial class MA_Bidding_Orders
    {
        public int ItemID { get; set; }
        public int OrderID { get; set; }
        public Nullable<int> BidID { get; set; }
        public Nullable<int> EventID { get; set; }
        public Nullable<int> Quantity { get; set; }
        public Nullable<decimal> PricePerQty { get; set; }
        public Nullable<decimal> AmountPaid { get; set; }
        public Nullable<decimal> TotalPrice { get; set; }
        public string ServiceName { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<int> VendorId { get; set; }
    
        public virtual MA_Admin_BiddingRequest MA_Admin_BiddingRequest { get; set; }
        public virtual MA_Events_Master MA_Events_Master { get; set; }
        public virtual MA_Orders_Master MA_Orders_Master { get; set; }
    }
}
