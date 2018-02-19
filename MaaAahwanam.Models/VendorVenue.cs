using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaaAahwanam.Models
{
    public class VendorVenue
    {
        public long Id { get; set; }
        public long VendorMasterId { get; set; }
        public string VenueType { get; set; }
        public string Food { get; set; }
        public string CockTails { get; set; }
        public string Rooms { get; set; }
        public int SeatingCapacity { get; set; }
        public int DiningCapacity { get; set; }
        public int Minimumseatingcapacity { get; set; }
        public int Maximumcapacity { get; set; }
        public decimal VegLunchCost { get; set; }
        public decimal NonVegLunchCost { get; set; }
        public decimal VegDinnerCost { get; set; }
        public decimal NonVegDinnerCost { get; set; }
        public string MinOrder { get; set; }
        public string MaxOrder { get; set; }
        public string DecorationAllowed { get; set; }
        public string HallType { get; set; }
        public string Wifi { get; set; }
        public string Menuwiththenoofitems { get; set; }
        public string Distancefrommainplaceslike { get; set; }
        public string LiveCookingStation { get; set; }
        public long ServiceCost { get; set; }
        public string Status { get; set; }
        public long UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string discount { get; set; }
        public string name { get; set; }
        public string Address { get; set; }
        public string Landmark { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string GeoLocation { get; set; }
        //public int tier { get; set; }
    }
}
