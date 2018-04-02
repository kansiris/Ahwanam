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
            ViewBag.count = 6;
            return View();
        }

        public ActionResult BlockOnePartial(string type, string f1, string f2, string f3, string f4, string f5, string f6, string f7, string f8, string f9, string L1)
        {
            int takecount = (L1 != null) ? int.Parse(L1) : 6;
            if (new string[] { "Mehendi", "Pandit" }.Contains(type))
            {
                f4 = (f4 == "undefined" && f4 == "" && int.Parse(f4) > 0) ? f4 : "1";
                ViewBag.type = type;
                if (f5 != "") f5 = f5.Split('-')[1].Trim(); else f5 = "100";
                var data = vendorProductsService.Getfiltervendors_Result(type, "", "", "", f4, f5, "", "", "", "");
                ViewBag.others = data.Take(takecount);
                int count = data.Count();
                ViewBag.count = (count >= takecount) ? "1" : "0";
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
            {
                type = (type == "Convetion") ? "Convention Hall" : type;
                var data = vendorProductsService.Getfiltervendors_Result(type, f1, f2, f3, f4, f5, f6, f7, f8, f9);
                ViewBag.venues = data.Take(takecount);
                ViewBag.type = type;
                int count = data.Count();
                ViewBag.count = (count >= takecount) ? "1" : "0";
            }
            else
            {
                var data = vendorProductsService.Getfiltervendors_Result("Venue", f1, f2, f3, f4, f5, f6, f7, f8, f9);
                ViewBag.venues = data.Take(takecount);
                ViewBag.type = "Venues";
                int count = data.Count();
                ViewBag.count = (count >= takecount) ? "1" : "0";
            }

            return PartialView();
        }

        public ActionResult BlockTwoPartial(string type, string f1, string f2, string f3, string f4, string f5, string f6, string f7, string f8, string f9,string L2)
        {
            int takecount = (L2 != null) ? int.Parse(L2) : 6;
            f2 = (f2 == "undefined" && f2 == "") ? f2.Split('-')[1].Trim() : "100";
            f3 = (f3 == "undefined" && f3 == "") ? f3 : "Yes";
            f5 = (f5 == "undefined" && f5 == "") ? f5 : "Yes";
            f6 = (f6 == "undefined" && f6 == "") ? f6 : "Yes";
            var data = vendorProductsService.Getfiltervendors_Result("Catering", "", f2, f3, "", f5, f6, "", "", "");
            ViewBag.Catering = data.Take(takecount);
            int count = data.Count();
            ViewBag.count = (count >= takecount) ? "1" : "0";
            return PartialView();
        }

        public ActionResult BlockThreePartial(string type, string f1, string f2, string f3, string f4, string f5, string f6, string f7, string f8, string f9, string L3)
        {
            int takecount = (L3 != null) ? int.Parse(L3) : 6;
            f2 = (f2 == "undefined" && f2 == "") ? f2 : "Yes";
            f3 = (f3 == "undefined" && f3 == "") ? f3 : "Yes";
            f4 = (f4 == "undefined" && f4 == "") ? f4 : "100";
            f5 = (f5 == "undefined" && f5 == "") ? f5 : "Yes";
            f6 = (f6 == "undefined" && f6 == "") ? f6 : "Yes";
            var data = vendorProductsService.Getfiltervendors_Result("Decorator", "", f2, f3, f4, f5, f6, "", "", "");
            ViewBag.Decorator = data.Take(takecount);
            int count = data.Count();
            ViewBag.count = (count >= takecount) ? "1" : "0";
            return PartialView();
        }

        public ActionResult BlockFourPartial(string type, string f1, string f2, string f3, string f4, string f5, string f6, string f7, string f8, string f9, string L4)
        {
            int takecount = (L4 != null) ? int.Parse(L4) : 6;
            f2 = (f2 == "undefined" && f2 == "") ? f2 : "Yes";
            f3 = (f3 == "undefined" && f3 == "") ? f3 : "Yes";
            f4 = (f4 == "undefined" && f4 == "") ? f4 : "100";
            var data = vendorProductsService.Getfiltervendors_Result("Photography", "", f2, f3, f4, "", "", "", "", "");
            ViewBag.Photography = data.Take(takecount);
            int count = data.Count();
            ViewBag.count = (count >= takecount) ? "1" : "0";
            return PartialView();
        }

        //public PartialViewResult Loadmore(string lastrecord)
        //{
        //    int id = (lastrecord == null) ? 6 : int.Parse(lastrecord) + 6;
        //    ViewBag.deal = vendorProductsService.getalldeal().OrderBy(m => m.DealID).Take(id);
        //    var deals = vendorProductsService.getalldeal().OrderBy(m => m.DealID).Take(id);
        //    ViewBag.dealLastRecord = id;
        //    ViewBag.dealcount = vendorProductsService.getalldeal().Count();
        //    return PartialView("Loadmore");
        //}

        public void parameterdescription()
        {
            //------------------ Venues ------------------------------

            //f1 ---> Food               checkbox
            //f2 ---> CockTail           radio
            //f3 ---> Rooms              radio
            //f4 ---> Seating Capacity   textbox
            //f5 ---> Maximum Order      radio
            //f6 ---> Decoration Allowed radio
            //f7 ---> Hall Type          checkbox
            //f8 ---> Wi-Fi              radio
            //f9 ---> Live Cooking Station            radio

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