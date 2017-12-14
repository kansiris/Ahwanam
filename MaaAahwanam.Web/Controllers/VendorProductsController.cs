using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaaAahwanam.Service;

namespace MaaAahwanam.Web.Controllers
{
    public class VendorProductsController : Controller
    {
        // GET: VendorProducts
        VendorProductsService vendorProductsService = new VendorProductsService();
        public ActionResult Index(string service)
        {
            if (service == "Hotels")
                ViewBag.records = vendorProductsService.Getvendorproducts_Result("Venue").Where(m=>m.subtype == "Hotel");
            else if (service == "Resorts")
                ViewBag.records = vendorProductsService.Getvendorproducts_Result("Venue").Where(m => m.subtype == "Resort");
            else if (service == "Conventions")
                ViewBag.records = vendorProductsService.Getvendorproducts_Result("Venue").Where(m => m.subtype == "Convention Hall");
            else
                ViewBag.records = vendorProductsService.Getvendorproducts_Result(service);
            return View();
        }
    }
}