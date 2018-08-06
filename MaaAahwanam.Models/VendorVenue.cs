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
        public string ComplimentaryBreakfast { get; set; }
        public string Geyser { get; set; }
        public string ParkingFacility { get; set; }
        public string CardPayment { get; set; }
        public string LiftorElevator { get; set; }
        public string BanquetHall { get; set; }
        public string Laundry { get; set; }
        public string CCTVCameras { get; set; }
        public string SwimmingPool { get; set; }
        public string ConferenceRoom { get; set; }
        public string Bar { get; set; }
        public string DiningArea { get; set; }
        public string PowerBackup { get; set; }
        public string WheelchairAccessible { get; set; }
        public string RoomHeater { get; set; }
        public string InRoomSafe { get; set; }
        public string MiniFridge { get; set; }
        public string InhouseRestaurant { get; set; }
        public string Gym { get; set; }
        public string HairDryer { get; set; }
        public string PetFriendly { get; set; }
        public string HDTV { get; set; }
        public string Spa { get; set; }
        public string WellnessCenter { get; set; }
        public string Electricity { get; set; }
        public string BathTub { get; set; }
        public string Kitchen { get; set; }
        public string Netflix { get; set; }
        public string Kindle { get; set; }
        public string CoffeeTeaMaker { get; set; }
        public string SofaSet { get; set; }
        public string Jacuzzi { get; set; }
        public string FullLengthMirrror { get; set; }
        public string Balcony { get; set; }
        public string KingBed { get; set; }
        public string QueenBed { get; set; }
        public string SingleBed { get; set; }
        public string Intercom { get; set; }
        public string SufficientRoomSize { get; set; }
        public string SufficientWashroom { get; set; }
    }
}
