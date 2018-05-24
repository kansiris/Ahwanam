using MaaAahwanam.Models;
using MaaAahwanam.Service;
using MaaAahwanam.Web.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MaaAahwanam.Web.Controllers
{
    public class NVendorProfileController : Controller
    {

        OrderService orderService = new OrderService();
        VendorMasterService vendorMasterService = new VendorMasterService();
        UserLoginDetailsService userLoginDetailsService = new UserLoginDetailsService();
        VenorVenueSignUpService venorVenueSignUpService = new VenorVenueSignUpService();
        // GET: NVendorProfile
        public ActionResult Index(string id)
        {

            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
                ViewBag.id = id;
                ViewBag.Vendor = vendorMasterService.GetVendor(long.Parse(id));
                var orders = orderService.userOrderList().Where(m => m.UserLoginId == (int)user.UserId);
                ViewBag.order = orders.OrderByDescending(m => m.OrderId);
                ViewBag.profilepic = userLoginDetailsService.GetUser(int.Parse(user.UserId.ToString())).UserImgName;
            }
            else
            {
                return RedirectToAction("Index", "NUserRegistration");
            }
            return View();
        }


        public ActionResult updateProfile([Bind(Prefix = "Item1")] Vendormaster vendor)
        {
            var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
            string email = vendor.EmailId;

            userLoginDetailsService.Updatevendordetailsnew(vendor, email);
            Int64 id = vendor.Id;
            TempData["Active"] = "Package added";
            return RedirectToAction("Index", "NVendorDashboard", new { id = id });
        }

       
    }
}