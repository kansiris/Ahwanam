using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaaAahwanam.Service;
using System.Net;
using MaaAahwanam.Models;
using MaaAahwanam.Utility;

namespace MaaAahwanam.Web.Controllers
{
    public class NResultsController : Controller
    {
        // GET: NResults
        VendorProductsService vendorProductsService = new VendorProductsService();
        public ActionResult Index(string f1, string f2, string f3, string f4, string f5, string f6, string f7, string f8, string f9)
        {
            //ViewBag.venues = vendorProductsService.Getfiltervendors_Result("Venue", f1, f2, f3, f4, f5.Split('-')[1].Trim(), f6, f7, f8, f9);//.Where(m=> (m.Food.Contains(f1) || m.Cocktail.Contains(f2) || m.Rooms.Contains(f3) || int.Parse(m.MaxGuest) <= int.Parse(f4) || int.Parse(m.Maxorder) <= int.Parse(f5) || m.Decoration.Contains(f6) || m.Halltype.Contains(f7) ||m.Wifi.Contains(f8)|| m.Livecooking.Contains(f9) ));
            //var hotelrecords = vendorProductsService.Getfiltervendors_Result("Hotel").Where(m => (m.Food == f1 || m.Cocktail == f2 || m.Rooms == f3 || int.Parse(m.MaxGuest) <= int.Parse(f4) || int.Parse(m.Maxorder) <= int.Parse(f5) || m.Decoration == f6 || m.Halltype == f7 || m.Wifi == f8 || m.Livecooking == f9));
            //var resortrecords = vendorProductsService.Getfiltervendors_Result("Resort").Where(m => (m.Food == f1 || m.Cocktail == f2 || m.Rooms == f3 || int.Parse(m.MaxGuest) <= int.Parse(f4) || int.Parse(m.Maxorder) <= int.Parse(f5) || m.Decoration == f6 || m.Halltype == f7 || m.Wifi == f8 || m.Livecooking == f9));
            //var conventionrecords = vendorProductsService.Getfiltervendors_Result("Convention Hall").Where(m => (m.Food == f1 || m.Cocktail == f2 || m.Rooms == f3 || int.Parse(m.MaxGuest) <= int.Parse(f4) || int.Parse(m.Maxorder) <= int.Parse(f5) || m.Decoration == f6 || m.Halltype == f7 || m.Wifi == f8 || m.Livecooking == f9));
            return View();
        }

        public ActionResult BlockOnePartial(string type, string f1, string f2, string f3, string f4, string f5, string f6, string f7, string f8, string f9, string count)
        {
            ViewBag.type = type;
            if (new string[] { "Mehendi", "Pandit" }.Contains(type))
            {
                f4 = (f4 == "undefined" && f4 == "" && int.Parse(f4) > 0) ? f4 : "1";

                if (f5 != "") f5 = f5.Split('-')[1].Trim(); else f5 = "100";
                ViewBag.venues = vendorProductsService.Getfiltervendors_Result(type, "", "", "", f4, f5, "", "", "", "").Take(6);
                return PartialView();
            }
            f1 = (f1 == "undefined" && f1 == "") ? f1 : "Yes";
                f2 = (f2 == "undefined" && f2 == "") ? f2 : "Yes";
                f3 = (f3 == "undefined" && f3 == "") ? f3 : "Yes";
                f4 = (f4 == "undefined" && f4 == "") ? f4 : "100";
                f5 = (f5 == "undefined" && f5 == "") ? f5.Split('-')[1].Trim() : "100";
                f6 = (f6 == "undefined" && f6 == "") ? f6 : "Yes";
                f7 = (f7 == "undefined" && f7 == "") ? f7 : "Yes";
                f8 = (f8 == "undefined" && f8 == "") ? f8 : "Yes";
                f9 = (f9 == "undefined" && f9 == "") ? f9 : "Yes";
            //count = 
            if (new string[] { "Hotel", "Resort", "Convetion" }.Contains(type))
                ViewBag.venues = vendorProductsService.Getfiltervendors_Result(type, f1, f2, f3, f4, f5, f6, f7, f8, f9).Take(6);
            else
                ViewBag.venues = vendorProductsService.Getfiltervendors_Result("Venue", f1, f2, f3, f4, f5, f6, f7, f8, f9).Take(6);
            
            return PartialView();
        }

        public ActionResult BlockTwoPartial(string type, string f1, string f2, string f3, string f4, string f5, string f6, string f7, string f8, string f9)
        {
            ViewBag.Catering = vendorProductsService.Getfiltervendors_Result("Catering", "", f2, f3, "", f5, f6, "", "", "");
            return PartialView();
        }

        public ActionResult BlockThreePartial(string type, string f1, string f2, string f3, string f4, string f5, string f6, string f7, string f8, string f9)
        {
            ViewBag.Decorator = vendorProductsService.Getfiltervendors_Result("Decorator", "", f2, f3, f4, f5, f6, "", "", "");
            return PartialView();
        }

        public ActionResult BlockFourPartial(string type, string f1, string f2, string f3, string f4, string f5, string f6, string f7, string f8, string f9)
        {
            ViewBag.Photography = vendorProductsService.Getfiltervendors_Result("Photography", "", f2, f3, f4, "", "", "", "", "");
            return PartialView();
        }

        public void parameterdescription()
        {
            //------------------ Venues ------------------------------

            //f1 ---> Food
            //f2 ---> CockTail
            //f3 ---> Rooms
            //f4 ---> Seating Capacity
            //f5 ---> Maximum Order
            //f6 ---> Decoration Allowed
            //f7 ---> Hall Type
            //f8 ---> Wi-Fi
            //f9 ---> Live Cooking Station

            //------------------ Catering ------------------------------

            //f2 ---> Maximum Order
            //f3 ---> Mineral Water included
            //f5 ---> Transport included
            //f6 ---> Live Cooking Station

            //------------------ Photography ------------------------------

            //f2 ---> Pre-Wedding Shoot
            //f3 ---> Destination Photography
            //f4 ---> Prior Booking Days

            //------------------ Decorator ------------------------------

            //f2 ---> Mandap
            //f3 ---> Mehendi
            //f5 ---> Sangeet
            //f6 ---> Lighting
            //f4 ---> Prior Booking Days

            //------------------ Other  ------------------------------

            //f1 ---> Maximum Order
            //f2 --->  Prior Booking Days

            //------------------ Event Management ------------------------------

            //f2 ---> Maximum Order
            //f3 ---> Type
            //f4 ---> Prior Booking Days

        }
    }
}