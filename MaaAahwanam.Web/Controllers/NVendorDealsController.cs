using MaaAahwanam.Models;
using MaaAahwanam.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MaaAahwanam.Web.Controllers
{
    public class NVendorDealsController : Controller
    {
        VenorVenueSignUpService vendorVenueSignUpService = new VenorVenueSignUpService();

        VendorProductsService vendorProductsService = new VendorProductsService();
        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

        // GET: NVendorDeals
        public ActionResult Index(string id)
        {
            if (TempData["Active"] != "")
            {
                ViewBag.msg = TempData["Active"];
            }
            var deals = vendorProductsService.getvendordeals(id);
            ViewBag.dealrecord = deals;
            ViewBag.id = id;
            return View();
        }
        public ActionResult edit(string pid, string vid)
        {
            var deals = vendorProductsService.getpartdeals(pid);
            ViewBag.dealrecord1 = deals;
            ViewBag.id = vid;
            return View();
        }

        public ActionResult updatedeal(string id, string vid, string DealName, string OriginalPrice, string DealPrice,string minGuests, string maxGuests, string StartDate, string EndDate, string ddesc , string timeslot, string timeslot1)
        {
            
            
            if (timeslot == null && timeslot1 == null || timeslot1 == "" || timeslot == "")
            {
                TempData["Active"] = "Please Login";
                return RedirectToAction("edit", "NVendorDeals", new { pid = id, vid = vid });

                //  return Content("<script> alert('select timeslot');location.href='" + @Url.Action("edit", "NVendorDeals", new { pid = id, vid = vid }) + "' </script>");
            }

            string time = null;
            if (timeslot == null || timeslot == "")
            { time = timeslot1; }

            if (timeslot1 == null || timeslot1 == "")
            { time = timeslot; }

            if (timeslot1 != null && timeslot != null)
            { time = timeslot + ',' + timeslot1; }

            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                DateTime updateddate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
                // DateTime updateddate = DateTime.Now;
                NDeals deals = new NDeals();
                deals.VendorId = Convert.ToInt64(vid);
                deals.DealName = DealName;
                deals.UpdatedDate = updateddate;
                deals.OriginalPrice = Decimal.Parse(OriginalPrice);
                deals.MinMemberCount = minGuests;
                deals.MaxMemberCount = maxGuests;
                deals.TimeSlot = time;
                deals.DealPrice = decimal.Parse(DealPrice);
                deals.DealStartDate = Convert.ToDateTime(StartDate);
                deals.DealEndDate = Convert.ToDateTime(EndDate);
                deals.DealDescription = ddesc;
               
                deals.TermsConditions = "TAXES EXTRA @ 18% PER PERSON / PER ROOM";
                deals = vendorVenueSignUpService.updatedeal(long.Parse(id),deals);
                TempData["Active"] = "Deal Updated";
                return RedirectToAction ("Index", "NVendorDeals", new { id = vid });
                //   return Content("<script language='javascript' type='text/javascript'>alert('deal updated');location.href='" + @Url.Action("Index", "NVendorDeals", new { id = vid }) + "'</script>");
            }
            TempData["Active"] = "Please Login";
            return RedirectToAction("Index", "Nhomepage", new { id = vid });
          //  return Content("<script language='javascript' type='text/javascript'>alert('Please login');location.href='" + @Url.Action("Index", "Nhomepage", new { id = vid }) + "'</script>");
        }

        public ActionResult deletedeal(string id, string vid)
        {

            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {

                string message = vendorVenueSignUpService.deletedeal(id);
                ViewBag.vendormasterid = id;
                if (message == "success")
                {
                    TempData["Active"] = "Deal Deleted";
                    return RedirectToAction("Index", "NVendorDeals", new { id = vid });
                    //return Content("<script type='text/javscript'> alert('package added'); location.href='/NVendorAddPackage/Index?id="+ id+ "</script>");
                    //  return Content("<script language='javascript' type='text/javascript'>alert('deal deleted');location.href='" + @Url.Action("Index", "NVendorDeals", new { id = vid }) + "'</script>");
                }
            }
            TempData["Active"] = "Please login";
            return RedirectToAction("Index", "Nhomepage", new { id = vid });
            //return Content("<script language='javascript' type='text/javascript'>alert('Please login');location.href='" + @Url.Action("Index", "Nhomepage", new { id = vid }) + "'</script>");
        }

    }
}