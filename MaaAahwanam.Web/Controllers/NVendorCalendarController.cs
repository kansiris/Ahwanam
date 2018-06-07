using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaaAahwanam.Service;
using MaaAahwanam.Models;
using MaaAahwanam.Web.Custom;

namespace MaaAahwanam.Web.Controllers
{
    public class NVendorCalendarController : Controller
    {
        VendorDatesService vendorDatesService = new VendorDatesService();
        VenorVenueSignUpService vendorVenueSignUpService = new VenorVenueSignUpService();
        UserLoginDetailsService userLoginDetailsService = new UserLoginDetailsService();
        ProductInfoService productInfoService = new ProductInfoService();
        // GET: NVendorCalendar
        public ActionResult Index(string id, string vid, string type)
        {
            try
            {
                ViewBag.id = id;
                ViewBag.vid = vid;
                if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
                    ViewBag.profilepic = userLoginDetailsService.GetUser(int.Parse(user.UserId.ToString())).UserImgName;
                    var orderdates = productInfoService.GetCount(long.Parse(id), long.Parse(vid), type);
                    string bookeddates = string.Empty;
                    List<string[]> userorderdates = new List<string[]>();
                    foreach (var item in orderdates)
                    {
                        bookeddates = item.BookedDate.ToString();
                        if (item.ExtraDate1 != "" && item.ExtraDate1 != null) bookeddates = bookeddates + "," + item.ExtraDate1.ToString();
                        if (item.ExtraDate2 != "" && item.ExtraDate2 != null) bookeddates = bookeddates + "," + item.ExtraDate2.ToString();
                        if (item.ExtraDate3 != "" && item.ExtraDate3 != null) bookeddates = bookeddates + "," + item.ExtraDate3.ToString();
                        var getuserdetails = userLoginDetailsService.GetUser(int.Parse(item.OrderBy.ToString()));
                        userorderdates.Add(new string[] { item.EventType, getuserdetails.FirstName, getuserdetails.LastName, bookeddates, item.attribute, item.Isdeal.ToString(),  getuserdetails.UserPhone });
                    }
                    ViewBag.userorderdates = userorderdates;
                    return View();
                }
                else
                    return RedirectToAction("Index", "NUserRegistration");
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Nhomepage");
            }
        }

        public JsonResult GetDates(string id, string vid, string type)
        {
            //var vendorsubcatids = vendorVenueSignUpService.GetVendorVenue(long.Parse(id));
            //long vendorsubid = (vid == null || vid == "undefined" || vid == "") ? vendorsubcatids.FirstOrDefault().Id : long.Parse(vid); // if vid is null then automatically first Subcategory ID will be considered
            var data = vendorDatesService.GetDates(long.Parse(id), long.Parse(vid));
            //if (orderdates != "") data = data + "," + orderdates.Split(',');
            return new JsonResult { Data = data, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpPost]
        public JsonResult DeleteEvent(string id)
        {
            var status = false;
            string msg = vendorDatesService.removedates(long.Parse(id));
            if (msg == "Removed") status = true;
            return new JsonResult { Data = new { status = status } };
        }

        [HttpPost]
        public JsonResult SaveEvent(VendorDates vendorDates, string vid)
        {
            //long vendorsubid = (vid == null || vid=="undefined"||vid=="")? vendorVenueSignUpService.GetVendorVenue(long.Parse(vendorDates.VendorId.ToString())).FirstOrDefault().Id : long.Parse(vid);
            var status = false;
            vendorDates.Vendorsubid = long.Parse(vid);
            if (vendorDates.Id > 0) //Update the event
                vendorDates = vendorDatesService.UpdatesVendorDates(vendorDates, vendorDates.Id);
            else //Save event
                vendorDates = vendorDatesService.SaveVendorDates(vendorDates);
            status = true;
            return new JsonResult { Data = new { status = status } };
        }

        [HttpGet]
        public JsonResult GetParticularDate(long id)
        {
            VendorDates dates = vendorDatesService.GetParticularDate(id);
            return new JsonResult { Data = dates, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        //public JsonResult CheckOrderDate(string date, string id, string vid, string type)
        //{
        //    ProductInfoService productInfoService = new ProductInfoService();
        //    string orderdates = productInfoService.disabledate(long.Parse(id), long.Parse(vid), type);
        //    if (orderdates.Split(',').Contains(date))
        //        return Json("true", JsonRequestBehavior.AllowGet);
        //    else
        //        return Json("false", JsonRequestBehavior.AllowGet);
        //}
    }
}