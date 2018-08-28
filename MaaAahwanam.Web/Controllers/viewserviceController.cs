using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaaAahwanam.Service;
using System.Web.Routing;
using MaaAahwanam.Web.Custom;
using MaaAahwanam.Models;
using MaaAahwanam.Utility;
using System.Net;
using System.IO;
using MaaAahwanam.Repository;

namespace MaaAahwanam.Web.Controllers
{
    public class viewserviceController : Controller
    {
        viewservicesservice viewservicesss = new viewservicesservice();
        //ProductInfoService productInfoService = new ProductInfoService();
        //WhishListService whishListService = new WhishListService();
        //UserLoginDetailsService userLoginDetailsService = new UserLoginDetailsService();
        //OrderService orderService = new OrderService();
        //CartService cartService = new CartService();
        //private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        ResultsPageService resultsPageService = new ResultsPageService();

        
        // GET: viewservice
        public ActionResult Index(string name, string type)
        {
            type = (type == null) ? "Venue" : type;
            type = (type == "Convention") ? "Convention Hall" : type;
            type = (type == "Banquet") ? "Banquet Hall" : type;
            type = (type == "Function") ? "Function Hall" : type;
            var ks = resultsPageService.GetAllVendors(type).Where(m => m.BusinessName.ToLower().Contains(name.ToLower().TrimEnd())).FirstOrDefault();
            string id = ks.Id.ToString();
            var venues = viewservicesss.GetVendorVenue(long.Parse(id)).ToList();
            if (type == "Venue" || type == "Convention" || type == "Banquet" || type == "Function")
            {
                var data = viewservicesss.GetVendor(long.Parse(id)); 
                var allimages = viewservicesss.GetVendorAllImages(long.Parse(id)).ToList();
                ViewBag.image = (allimages.Count() != 0) ? allimages.FirstOrDefault().ImageName.Replace(" ", "") : null;
                ViewBag.allimages = allimages;
                ViewBag.Productinfo = data;
                ViewBag.latitude = (data.GeoLocation != null && data.GeoLocation != "") ? data.GeoLocation.Split(',')[0] : "17.385044";
                ViewBag.longitude = (data.GeoLocation != null && data.GeoLocation != "") ? data.GeoLocation.Split(',')[1] : "78.486671";

                //var data = productInfoService.getProductsInfo_Result(int.Parse(id), type, int.Parse(vid));
                ViewBag.data = data;
                List<VendorVenue> vendor = venues;
                List<SPGETNpkg_Result> package = new List<SPGETNpkg_Result>();

                foreach (var item in venues)
                {
                    package.AddRange(viewservicesss.getvendorpkgs(id).Where(p => p.VendorSubId == long.Parse(item.Id.ToString())).ToList());
                }
                ViewBag.particularVenue = vendor;
                ViewBag.availablepackages = package;
                if (type.Split(',').Count() > 1) type = type.Split(',')[0];
                if (type == "Venues" || type == "Banquet Hall" || type == "Function Hall") type = "Venue";
            }
            return View();
        }

        public void Amenities()
        {
            List<VendorVenue> loadrecords = new List<VendorVenue>();
            List<VendorVenue> final = new List<VendorVenue>();
            var availableamenities = loadrecords.Select(m => new
            {
                m.AC,
                m.TV,
                m.ComplimentaryBreakfast,
                m.Geyser,
                m.ParkingFacility,
                m.CardPayment,
                m.LiftorElevator,
                m.BanquetHall,
                m.Laundry,
                m.CCTVCameras,
                m.SwimmingPool,
                m.ConferenceRoom,
                m.Bar,
                m.DiningArea,
                m.PowerBackup,
                m.WheelchairAccessible,
                m.RoomHeater,
                m.InRoomSafe,
                m.MiniFridge,
                m.InhouseRestaurant,
                m.Gym,
                m.HairDryer,
                m.PetFriendly,
                m.HDTV,
                m.Spa,
                m.WellnessCenter,
                m.Electricity,
                m.BathTub,
                m.Kitchen,
                m.Netflix,
                m.Kindle,
                m.CoffeeTeaMaker,
                m.SofaSet,
                m.Jacuzzi,
                m.FullLengthMirrror,
                m.Balcony,
                m.KingBed,
                m.QueenBed,
                m.SingleBed,
                m.Intercom,
                m.SufficientRoomSize,
                m.SufficientWashroom
            });
            final.AddRange(loadrecords);
        }
    }
}