using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaaAahwanam.Service;
using MaaAahwanam.Repository;

namespace MaaAahwanam.Web.Controllers
{
    public class LiveDealsController : Controller
    {
        // GET: LiveDeals
        public ActionResult Index(string search, string type)
        {
            VendorProductsService vendorProductsService = new VendorProductsService();
            ViewBag.search = search;
            ViewBag.Venue = vendorProductsService.getdealsearch(search, "Venue");
            ViewBag.Hotels = vendorProductsService.getdealsearch(search, "Hotel");
            ViewBag.Resorts = vendorProductsService.getdealsearch(search, "Resort");
            ViewBag.Conventions = vendorProductsService.getdealsearch(search, "Convention Hall");
            ViewBag.Catering = vendorProductsService.getdealsearch(search, "Catering");
            ViewBag.Photography = vendorProductsService.getdealsearch(search, "Photography");
            ViewBag.Decorator = vendorProductsService.getdealsearch(search, "Decorator");
            ViewBag.Mehendi = vendorProductsService.getdealsearch(search, "Mehendi");
            return View();
        }
    }
}