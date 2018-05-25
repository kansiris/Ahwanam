using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaaAahwanam.Service;
using MaaAahwanam.Models;
using MaaAahwanam.Web.Custom;
using MaaAahwanam.Service;

namespace MaaAahwanam.Web.Controllers
{
    public class NVendorDashboardController : Controller
    {
        OrderService orderService = new OrderService();
        VendorMasterService vendorMasterService = new VendorMasterService();
        UserLoginDetailsService userLoginDetailsService = new UserLoginDetailsService();

        // GET: NVendorDashboard
        public ActionResult Index(string id)
        {
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
                ViewBag.id = id;
                ViewBag.Vendor = vendorMasterService.GetVendor(long.Parse(id));
                var orders = orderService.userOrderList().Where(m => m.Id == int.Parse(id));
                ViewBag.currentorders = orders.Where(p=>p.Status == "Pending").Count();
                ViewBag.ordershistory = orders.Where(m => m.Status != "Removed").Count();
                ViewBag.profilepic = userLoginDetailsService.GetUser(int.Parse(user.UserId.ToString())).UserImgName;
            }
            else
            {
                return RedirectToAction("Index", "NUserRegistration");
            }
            return View();
        }

        public PartialViewResult VendorAuth()
        {
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
                var vendorrecord = userLoginDetailsService.GetUser(int.Parse(user.UserId.ToString()));
                ViewBag.profilepic = vendorrecord.UserImgName;
                var emailid = vendorrecord.AlternativeEmailID;
                ViewBag.id = vendorMasterService.GetVendorByEmail(emailid).Id;
            }
            return PartialView("VendorAuth");
        }
    }
}