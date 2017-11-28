using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaaAahwanam.Models
{

    public class VendorsDecorator
    {
        [Key]
        public long Id { get; set; }
        public long VendorMasterId { get; set; }
        public string DecorationType { get; set; }
        public decimal StartingPrice { get; set; }
        public string Status { get; set; }
        public long UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string Archway { get; set; }
        public string Altararrangements { get; set; }
        public string Pewbows { get; set; }
        public string Aislerunner { get; set; }
        public string Headpieces { get; set; }
        public string Centerpieces { get; set; }
        public string Chaircovers { get; set; }
        public string Headtabledecor { get; set; }
        public string Backdrops { get; set; }
        public string Ceilingcanopies { get; set; }
        public string Mandaps { get; set; }
        public string Mehendi { get; set; }
        public string Sangeet { get; set; }
        public string Chuppas { get; set; }
        public string Lighting { get; set; }
        public string Giftsforguests { get; set; }
        public string Gifttable { get; set; }
        public string BasketorBoxforgifts { get; set; }
        public string Placeorseatingcards { get; set; }
        public string Cardecoration { get; set; }
        public string Bridesbouquet { get; set; }
        public string Bridesmaidsbouquets { get; set; }
        public string Maidofhonorbouquet { get; set; }
        public string Throwawaybouquet { get; set; }
        public string Corsages { get; set; }
        public string Boutonnieres { get; set; }
        public string Decora { get; set; }
        public string Justmarriedclings { get; set; }
        public string discount { get; set; }
    }
}
