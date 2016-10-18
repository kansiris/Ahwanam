using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaaAahwanam.Service;
using MaaAahwanam.Models;

namespace MaaAahwanam.Web.Areas.Admin.Controllers
{
    public class DealsController : Controller
    {
        VendorSetupService vendorSetupService = new VendorSetupService();
        // GET: Admin/Deals
        public ActionResult AllDeals()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AllDeals(string dropstatus,string command, string id, string vid)
        {
            if (dropstatus != null && dropstatus != "")
            {
                ViewBag.VendorList = vendorSetupService.AllVendorList(dropstatus);
            }
            //if (command == "Make A Deal")
            //{
            //    return RedirectToAction(dropstatus, "CreateVendor", new { id = id, vid = vid, op = "display" });
            //}
            return View();
        }
        
        public ActionResult NewDeal()
        {
            return View();
        }
    }
}