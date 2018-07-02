using MaaAahwanam.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MaaAahwanam.Web.Controllers
{
    public class NVendorsController : Controller
    {
        // GET: NVendors
        VendorProductsService vendorProductsService = new VendorProductsService();
        public ActionResult Index(string servicetype)
        {
            ViewBag.service = servicetype;
            if (servicetype == "Hotels")
                ViewBag.records = vendorProductsService.Getvendorproducts_Result("Hotel");//.Where(m => m.subtype == "Hotel");
            else if (servicetype == "Resorts")
                ViewBag.records = vendorProductsService.Getvendorproducts_Result("Resort");//.Where(m => m.subtype == "Resort");
            else if (servicetype == "Conventions")
                ViewBag.records = vendorProductsService.Getvendorproducts_Result("Convention Hall");//.Where(m => m.subtype == "Convention Hall");
            else
                ViewBag.records = vendorProductsService.Getvendorproducts_Result(servicetype);
            return View();
        }
    }
}