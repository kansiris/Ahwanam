using MaaAahwanam.Models;
using MaaAahwanam.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MaaAahwanam.Web.Controllers
{
    public class NewVendorDashboardController : Controller
    {
        VenorVenueSignUpService venorVenueSignUpService = new VenorVenueSignUpService();
        Vendormaster vendorMaster = new Vendormaster();
        VendorMasterService vendorMasterService = new VendorMasterService();
        // GET: NewVendorDashboard
        public ActionResult Index(string id)
        {
            ViewBag.id = id;
            //vendorMaster = vendorMasterService.GetVendor(long.Parse(id));
            return View();
        }
    }
}