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
    public class NVendorDealsController : Controller
    {
        VenorVenueSignUpService vendorVenueSignUpService = new VenorVenueSignUpService();

        VendorProductsService vendorProductsService = new VendorProductsService();
        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

        // GET: NVendorDeals
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
                arrIndMsg = arrMsgs[0].Split('='); //Get the id
                id = arrIndMsg[1].ToString().Trim();
                var deals = vendorProductsService.getvendordeals(id);
            ViewBag.dealrecord = deals;
            ViewBag.id = ks;
            return View();
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Nhomepage");
            }
        }
        public ActionResult edit(string pid, string vid)
        {
            try { 
            var deals = vendorProductsService.getpartdeals(pid);
            ViewBag.dealrecord1 = deals;
                string vssid = Convert.ToString(vid);
                encptdecpt encript = new encptdecpt();

                string encripted = encript.Encrypt(string.Format("Name={0}", vssid));
                ViewBag.id = encripted;
            return View();
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Nhomepage");
            }
        }

        public ActionResult updatedeal(string id, string vid, string DealName, string OriginalPrice, string DealPrice,string minGuests, string maxGuests, string StartDate, string EndDate, string ddesc , string timeslot, string timeslot1)
        {
            try { 
            //if (timeslot == null && timeslot1 == null || timeslot1 == "" || timeslot == "")
            //{
            //    TempData["Active"] = "Please Login";
            //    return RedirectToAction("edit", "NVendorDeals", new { pid = id, vid = vid });
            //}
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
                NDeals deals = new NDeals();
                deals.VendorId = Convert.ToInt64(vid);
                deals.DealName = DealName;
                deals.UpdatedDate = updateddate;
                deals.OriginalPrice = Decimal.Parse(OriginalPrice);
                deals.MinMemberCount = minGuests;
                deals.MaxMemberCount = maxGuests;
                deals.TimeSlot = time;
                deals.DealPrice = decimal.Parse(DealPrice);
                deals.DealStartDate = TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(StartDate), INDIAN_ZONE);
                deals.DealEndDate = TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(EndDate), INDIAN_ZONE);
                deals.DealDescription = ddesc;
                deals.TermsConditions = "TAXES EXTRA @ 18% PER PERSON / PER ROOM";
                deals = vendorVenueSignUpService.updatedeal(long.Parse(id),deals);
                    //  TempData["Active"] = "Deal Updated";
                    //  return RedirectToAction ("Index", "NVendorDeals", new { id = vid });
                    string vssid = Convert.ToString(vid);
                    encptdecpt encript = new encptdecpt();

                    string encripted = encript.Encrypt(string.Format("Name={0}", vssid));
                    return Content("<script language='javascript' type='text/javascript'>alert('Deal Updated successfully');location.href='" + @Url.Action("Index", "NVendorDeals", new { ks = encripted }) + "'</script>");

                }
            return Content ("<script> alert('Please Login');location.href='"+ @Url.Action("Index","Nhomepage",new { id = vid})+"'</script>");
            //    TempData["Active"] = "Please Login";
            //return RedirectToAction("Index", "Nhomepage", new { id = vid });
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Nhomepage");
            }
        }

        public ActionResult deletedeal(string id, string ks)
        {
            try { 
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {

                string message = vendorVenueSignUpService.deletedeal(id);
                    
                    ViewBag.vendormasterid = id;
                if (message == "success")
                {
                        return Content("<script> alert('Deal Deleted');location.href='" + @Url.Action("Index", "NVendorDeals", new { ks = ks }) + "'</script>");

                      //  TempData["Active"] = "Deal Deleted";
                   // return RedirectToAction("Index", "NVendorDeals", new { id = vid });
                }
            }
                return Content("<script> alert('Please login');location.href='" + @Url.Action("Index", "Nhomepage") + "'</script>");

            //    TempData["Active"] = "Please login";
            //return RedirectToAction("Index", "Nhomepage", new { id = vid });
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Nhomepage");
            }
        }

    }
}