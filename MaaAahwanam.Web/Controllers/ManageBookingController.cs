using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaaAahwanam.Service;
using MaaAahwanam.Models;
using MaaAahwanam.Web.Custom;
using MaaAahwanam.Utility;

namespace MaaAahwanam.Web.Controllers
{
    public class ManageBookingController : Controller
    {
        VendorMasterService vendorMasterService = new VendorMasterService();
        VendorProductsService vendorProductsService = new VendorProductsService();
        UserLoginDetailsService userLoginDetailsService = new UserLoginDetailsService();
        ProductInfoService productInfoService = new ProductInfoService();
        // GET: ManageBooking
        public ActionResult Index()
        {
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {

                var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
                var vendordetails = vendorMasterService.GetVendorServiceType(user.UserId);
                long vendorid = vendordetails.Id;
                encptdecpt encript = new encptdecpt();

                string encripted = encript.Encrypt(string.Format("Name={0}", vendorid));
                ViewBag.id = encripted;
                string vendortype = vendordetails.ServicType;
                var deals = vendorProductsService.getvendorsubid(vendorid.ToString());
                ViewBag.venuerecord = deals;
                ViewBag.profilepic = userLoginDetailsService.GetUser(int.Parse(user.UserId.ToString())).UserImgName;
                return View();
            }
            else
            {
                return RedirectToAction("SignOut", "NUserRegistration");
            }
        }

        public JsonResult CheckAvailability(string id, string subvid, string type,string date)
        {
            var orderdates = productInfoService.GetCount(long.Parse(id), long.Parse(subvid), type);
            string bookeddates = string.Empty;
            List<string[]> userorderdates = new List<string[]>();
            foreach (var item in orderdates)
            {
                bookeddates = item.BookedDate.ToString();
                if (item.ExtraDate1 != "" && item.ExtraDate1 != null) bookeddates = bookeddates + "," + item.ExtraDate1.ToString();
                if (item.ExtraDate2 != "" && item.ExtraDate2 != null) bookeddates = bookeddates + "," + item.ExtraDate2.ToString();
                if (item.ExtraDate3 != "" && item.ExtraDate3 != null) bookeddates = bookeddates + "," + item.ExtraDate3.ToString();
                var getuserdetails = userLoginDetailsService.GetUser(int.Parse(item.OrderBy.ToString()));
                userorderdates.Add(new string[] { item.EventType, getuserdetails.FirstName, getuserdetails.LastName, bookeddates, item.attribute, item.Isdeal.ToString(), getuserdetails.UserPhone });
                if (bookeddates.Split(',').Contains(date))
                    return Json("Date Blocked by " + getuserdetails.FirstName + " " + getuserdetails.LastName + "", JsonRequestBehavior.AllowGet);
            }
            ViewBag.userorderdates = userorderdates;
            //orderdates = "2018-08-01";
            return Json("Valid",JsonRequestBehavior.AllowGet);
        }
    }
}