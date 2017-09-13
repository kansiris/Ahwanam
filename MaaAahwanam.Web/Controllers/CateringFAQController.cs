﻿using MaaAahwanam.Models;
using MaaAahwanam.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MaaAahwanam.Web.Controllers
{
    public class CateringFAQController : Controller
    {
        Vendormaster vendorMaster = new Vendormaster();
        VenorVenueSignUpService venorVenueSignUpService = new VenorVenueSignUpService();
        VendorCateringService vendorCateringService = new VendorCateringService();
        // GET: CateringFAQ
        public ActionResult Index(string id, string vid)
        {
            ViewBag.id = id;
            ViewBag.vid = vid;
            return View();
        }

        [HttpPost]
        public ActionResult Index(string id, string vid, VendorsCatering vendorsCatering)
        {
            var data = vendorCateringService.GetVendorCatering(long.Parse(id), long.Parse(vid));
            vendorsCatering.UpdatedBy = 2;
            vendorMaster.UpdatedBy = 2;
            vendorsCatering.CuisineType = data.CuisineType;
            long masterid = vendorsCatering.VendorMasterId = vendorMaster.Id = long.Parse(id);
            vendorsCatering = venorVenueSignUpService.UpdateCatering(vendorsCatering, vendorMaster, masterid, long.Parse(vid));
            //return RedirectToAction("Index", "VendorSignUp4", new { id = id, vid = vid });
            //return Content("<script language='javascript' type='text/javascript'>alert('FAQ's Updated');location.href='" + @Url.Action("Index", "VendorSignUp4", new { id = id, vid = vid }) + "'</script>");
            return Content("<script language='javascript' type='text/javascript'>alert('FAQs Updated');location.href='" + @Url.Action("Index", "AvailableServices", new { id = vid }) + "'</script>");
        }
    }
}