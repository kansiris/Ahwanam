using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaaAahwanam.Models;
using MaaAahwanam.Service;

namespace MaaAahwanam.Web.Controllers
{
    public class VendorSignUp4Controller : Controller
    {
        Vendormaster vendorMaster = new Vendormaster();
        VenorVenueSignUpService venorVenueSignUpService = new VenorVenueSignUpService();
        VendorMasterService vendorMasterService = new VendorMasterService();
        // GET: VendorSignUp4
        public ActionResult Index(string id, string vid)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string id, string vid,string discount)
        {
            vendorMaster = vendorMasterService.GetVendor(long.Parse(id));
            vendorMaster.discount = discount;
            vendorMaster = vendorMasterService.UpdateVendorMaster(vendorMaster, long.Parse(id));
            return RedirectToAction("Index", "VendorSeccessReg");
        }
    }
}