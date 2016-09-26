using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaaAahwanam.Service;
using MaaAahwanam.Models;
using MaaAahwanam.Utility;
using System.IO;

namespace MaaAahwanam.Web.Areas.Admin.Controllers
{
    public class VendorsController : Controller
    {
        
        //VendorVenueService vendorVenueService = new VendorVenueService();
        //VendorImageService vendorImageService = new VendorImageService();
        VendorMasterService vendorMasterService = new VendorMasterService();
        VendorSetupService vendorSetupService = new VendorSetupService();
        public ActionResult AllVendors()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AllVendors(string dropstatus,string vid,string command,string id,string type,[Bind(Prefix = "Item2")] VendorVenue vendorVenue, [Bind(Prefix = "Item1")] Vendormaster vendorMaster)
        {
            if (dropstatus != null && dropstatus != "")
            {
                ViewBag.VendorList = vendorSetupService.AllVendorList(dropstatus);
            }
            if (command == "Edit")
            {
                return RedirectToAction(dropstatus, "CreateVendor", new { id = id, vid = vid });
            }
            if (command == "View")
            {
                return RedirectToAction(dropstatus, "CreateVendor", new { id = id, vid = vid, op = "display" });
            }
            if (command == "Add New")
            {
                return RedirectToAction(dropstatus, "CreateVendor", new { id = id, vid = vid, op = "add" });
            }
            return View();
        }
        public ActionResult ActiveVendors()
        {
            return View();
        }
        public ActionResult PendingVendors()
        {
            return View();
        }
        public ActionResult SuspendedVendors()
        {
            return View();
        }
        public ActionResult VendorDetails(string id, [Bind(Prefix = "Item2")] VendorVenue vendorVenue, [Bind(Prefix = "Item1")] Vendormaster vendorMaster)
        {
            return View();
        }
	}
}