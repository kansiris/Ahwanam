﻿using System;
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
        //VendorMasterService vendorMasterService = new VendorMasterService();
        VendorSetupService vendorSetupService = new VendorSetupService();
        public ActionResult AllVendors(string dropdown)
        {
            if (dropdown!=null)
            {
                ViewBag.VendorList = vendorSetupService.AllVendorList(dropdown);
            }
            return View();
        }
        [HttpPost]
        public ActionResult AllVendors(string dropstatus,string command,string id,string type,[Bind(Prefix = "Item2")] VendorVenue vendorVenue, [Bind(Prefix = "Item1")] Vendormaster vendorMaster)
        {
            if (dropstatus != null)
            {
                ViewBag.VendorList = vendorSetupService.AllVendorList(dropstatus);
            }
            if (command == "Edit")
            {
                return RedirectToAction(dropstatus, "CreateVendor", new { id = id});
            }
            if (command == "View")
            {
                return RedirectToAction(dropstatus, "CreateVendor", new { id = id,op = "display" });
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
            //if (id!=null)
            //{
            //    vendorVenue = vendorVenueService.GetVendorVenue(long.Parse(id));
            //    vendorMaster = vendorMasterService.GetVendor(long.Parse(id));
            //    var a = new Tuple<Vendormaster, VendorVenue>(vendorMaster, vendorVenue);
            //    ViewBag.images = vendorImageService.GetVendorImagesService(long.Parse(id));
            //    return View(a);
            //}
            return View();
        }
	}
}