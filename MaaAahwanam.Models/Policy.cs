using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaaAahwanam.Models
{
    public class Policy
    {
        [Key]
        public long Id { get; set; }
        public string ServiceType { get; set; }
        public string VendorId { get; set; }
        public string VendorSubId { get; set; }
        public string Outside_decorators_allowed { get; set; }
        public string Decor_provided { get; set; }
        public string Decorators_allowed_with_royalty { get; set; }
        public string Decoration_starting_costs { get; set; }
        public string Food_provided { get; set; }
        public string Outside_food_or_caterer_allowed { get; set; }
        public string NonVeg_allowed { get; set; }
        public string Alcohol_allowed { get; set; }
        public string Outside_Alcohol_allowed { get; set; }
        public string Tax { get; set; }
        public string Valet_Parking { get; set; }
        public string Parking_Space { get; set; }
        public string Cancellation { get; set; }
        public string Advance_Amount { get; set; }
        public string Available_Rooms { get; set; }
        public string Rooms_Count { get; set; }
        public string Room_Average_Price { get; set; }
        public string Changing_Rooms_AC { get; set; }
        public string Complimentary_Changing_Room { get; set; }
        public string Music_Allowed_Late { get; set; }
        public string Halls_AC { get; set; }
        public string Ample_Parking { get; set; }
        public string Baarat_Allowed { get; set; }
        public string Fire_Crackers_Allowed { get; set; }
        public string Hawan_Allowed { get; set; }
        public string Overnight_wedding_Allowed { get; set; }
    }
}
