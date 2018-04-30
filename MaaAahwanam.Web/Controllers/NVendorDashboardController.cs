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
        // GET: NVendorDashboard
        public ActionResult Index(string id)
        {
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
                ViewBag.id = id;
                ViewBag.Vendor = vendorMasterService.GetVendor(long.Parse(id));
                var orders = orderService.userOrderList().Where(m => m.UserLoginId == (int)user.UserId);
                ViewBag.order = orders.OrderByDescending(m => m.OrderId);
            }
            else
            {
                return RedirectToAction("Index", "NUserRegistration");
            }
            return View();
        }
    }
}