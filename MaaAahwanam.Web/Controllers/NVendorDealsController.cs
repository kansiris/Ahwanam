using MaaAahwanam.Models;
using MaaAahwanam.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MaaAahwanam.Web.Controllers
{
    public class NVendorDealsController : Controller
    {
        VendorProductsService vendorProductsService = new VendorProductsService();

        // GET: NVendorDeals
        public ActionResult Index(string id)
        {
            var deals = vendorProductsService.getvendordeals(id);
            ViewBag.dealrecord = deals;
            ViewBag.id = id;
            return View();
        }
    }
}