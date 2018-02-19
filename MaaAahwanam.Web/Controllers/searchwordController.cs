using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaaAahwanam.Service;
using MaaAahwanam.Repository;

namespace MaaAahwanam.Web.Controllers
{
    public class searchwordController : Controller
    {
        // GET: searchword
        public ActionResult Index(string search, string type)
        {
            VendorProductsService vendorProductsService = new VendorProductsService();
            ViewBag.search = search;


            ViewBag.Venue = vendorProductsService.getwordsearch(search, type = "Venue");
            ViewBag.Hotels = vendorProductsService.getwordsearch(search, type = "Hotel");
            ViewBag.Resorts = vendorProductsService.getwordsearch(search, type = "Resort");
            ViewBag.Conventions = vendorProductsService.getwordsearch(search, type = "Convention Hall");
            ViewBag.Catering = vendorProductsService.getwordsearch(search, type = "Catering");
            ViewBag.Photography = vendorProductsService.getwordsearch(search, type = "Photography");
            ViewBag.Decorator = vendorProductsService.getwordsearch(search, type = "Decorator");
            ViewBag.Mehendi = vendorProductsService.getwordsearch(search, type = "Mehendi");


            return View();
        }
    }
}