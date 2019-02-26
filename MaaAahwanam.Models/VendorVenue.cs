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
        public string Dimentions { get; set; }
        public string Description { get; set; }
        public string AC { get; set; }
        public string TV { get; set; }
        public string Complimentary_Breakfast { get; set; }
        public string Geyser { get; set; }
        public string Parking_Facility { get; set; }
        public string Card_Payment { get; set; }
        public string Lift_or_Elevator { get; set; }
        public string Banquet_Hall { get; set; }
        public string Laundry { get; set; }
        public string CCTV_Cameras { get; set; }
        public string Swimming_Pool { get; set; }
        public string Conference_Room { get; set; }
        public string Bar { get; set; }
        public string Dining_Area { get; set; }
        public string Power_Backup { get; set; }
        public string Wheelchair_Accessible { get; set; }
        public string Room_Heater { get; set; }
        public string In_Room_Safe { get; set; }
        public string Mini_Fridge { get; set; }
        public string In_house_Restaurant { get; set; }
        public string Gym { get; set; }
        public string Hair_Dryer { get; set; }
        public string Pet_Friendly { get; set; }
        public string HDTV { get; set; }
        public string Spa { get; set; }
        public string Wellness_Center { get; set; }
        public string Electricity { get; set; }
        public string Bath_Tub { get; set; }
        public string Kitchen { get; set; }
        public string Netflix { get; set; }
        public string Kindle { get; set; }
        public string Coffee_Tea_Maker { get; set; }
        public string Sofa_Set { get; set; }
        public string Jacuzzi { get; set; }
        public string Full_Length_Mirrror { get; set; }
        public string Balcony { get; set; }
        public string King_Bed { get; set; }
        public string Queen_Bed { get; set; }
        public string Single_Bed { get; set; }
        public string Intercom { get; set; }
        public string Sufficient_Room_Size { get; set; }
        public string Sufficient_Washroom { get; set; }
        public string page_name { get; set; }
    }
}
