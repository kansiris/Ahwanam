//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MaaAahwanam.Repository
{
    using System;
    
    public partial class sp_AllOrders_Result
    {
        public long OrderId { get; set; }
        public string servictype { get; set; }
        public Nullable<System.DateTime> OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; }
    }
}
