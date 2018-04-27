using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaaAahwanam.Service;
using MaaAahwanam.Models;

namespace MaaAahwanam.Web.Controllers
{
    public class NVendorDashboardController : Controller
    {
        VendorMasterService vendorMasterService = new VendorMasterService();
        // GET: NVendorDashboard
        public ActionResult Index(string id)
        {
            ViewBag.id = id;
            ViewBag.Vendor = vendorMasterService.GetVendor(long.Parse(id));
            return View();
        }
    }
}