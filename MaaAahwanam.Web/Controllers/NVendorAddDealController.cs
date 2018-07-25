using MaaAahwanam.Models;
using MaaAahwanam.Service;
using MaaAahwanam.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MaaAahwanam.Web.Controllers
{
    public class NVendorAddDealController : Controller
    {
        // GET: NVendorAddDeal
        VenorVenueSignUpService vendorVenueSignUpService = new VenorVenueSignUpService();
        VendorProductsService vendorProductsService = new VendorProductsService();
        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");


        public ActionResult Index(string ks)
        {
            try { 
            if (TempData["Active"] != "")
            {
                ViewBag.msg = TempData["Active"];
            }
             string strReq = "";
                encptdecpt encript = new encptdecpt();
                strReq = encript.Decrypt(ks);
                //Parse the value... this is done is very raw format.. you can add loops or so to get the values out of the query string...
                string[] arrMsgs = strReq.Split('&');


                string[] arrIndMsg;
                string id = "";
                arrIndMsg = arrMsgs[0].Split('='); //Get the Name
                id = arrIndMsg[1].ToString().Trim();
            var deals = vendorProductsService.getvendorsubid(id);
            ViewBag.venuerecord = deals;
            ViewBag.vendormasterid = id;
            ViewBag.id = ks;
            return View();
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Nhomepage");
            }
        }

        public ActionResult adddeal(string id, string DealName, string OriginalPrice, string type, string foodtype, string DealPrice, string catogary,string minGuests,string maxGuests, string StartDate, string EndDate, string ddesc, string timeslot,string timeslot1)
        {
            try { 
            if (type == "Select Type")
            {
                TempData["Active"] = "Select Type";
                return RedirectToAction("Index", "NVendorAddDeal",  new { id = id });
            }
            if (catogary == "Select Event")
            {
                TempData["Active"] = "Select Event";
                return RedirectToAction("Index", "NVendorAddDeal", new { id = id });
            }

            //if (timeslot == null && timeslot1 == null || timeslot1 == "" || timeslot == "")
            //{
            //    TempData["Active"] = "Select Timeslot";
            //    return RedirectToAction("Index", "NVendorAddDeal", new { id = id });
            //}

            DateTime s = Convert.ToDateTime(StartDate);
            DateTime e = Convert.ToDateTime(EndDate);

            if (e < s)
            {
                TempData["Active"] = "Start Date should be lesser than End Date";
                return RedirectToAction("Index", "NVendorAddDeal", new { id = id });
            }
            string time = null;
            if (timeslot == null || timeslot == "")
            { time = timeslot1; }

            if (timeslot1 == null || timeslot1 == "")
            { time = timeslot; }

            if (timeslot1 != null && timeslot != null)
            { time = timeslot + ',' + timeslot1; }
            if (timeslot == null && timeslot1 == null || timeslot1 == "" || timeslot == "")
            {
                time = "Morning,Evening";
            }
                DateTime date = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
            string[] word = type.Split(',');
            string subid = word[0];
            string type1 = word[1];
            string subtype = word[2];
            NDeals deals = new NDeals();
            deals.DealName = DealName;
            deals.TimeSlot = time;
            deals.VendorId = Convert.ToInt64(id);
            deals.VendorSubId = Convert.ToInt64(subid);
            deals.VendorSubType = subtype;
            deals.VendorType = type1;
            deals.UpdatedDate = date;
            deals.OriginalPrice = Decimal.Parse(OriginalPrice);
            deals.MinMemberCount = minGuests;
            deals.MaxMemberCount = maxGuests;
            if (type1 == "Venue" || type1 == "Catering")
            { 
            deals.FoodType = foodtype; }

            deals.DealPrice =  decimal.Parse(DealPrice);
            deals.DealStartDate = (Convert.ToDateTime(StartDate));
            deals.DealEndDate = (Convert.ToDateTime(EndDate));
            deals.DealDescription = ddesc;
            deals.Category = catogary;
            deals.TermsConditions = "TAXES EXTRA @ 18% PER PERSON / PER ROOM";
            deals = vendorVenueSignUpService.adddeal(deals);
            ViewBag.id = id;
            TempData["Active"] = "Deal is Saved";
                string vssid = Convert.ToString(id);
                encptdecpt encript = new encptdecpt();

                string encripted = encript.Encrypt(string.Format("Name={0}", vssid));
                return RedirectToAction("Index", "NVendorDeals", new { ks = encripted });
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Nhomepage");
            }
        }
      }
}