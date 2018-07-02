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
            { ViewBag.records = vendorProductsService.Getvendorproducts_Result("Hotel"); ViewBag.service = "Hotel"; }
            else if (servicetype == "Resorts")
            { ViewBag.records = vendorProductsService.Getvendorproducts_Result("Resort"); ViewBag.service = "Resort"; }
            else if (servicetype == "Conventions")
            { ViewBag.records = vendorProductsService.Getvendorproducts_Result("Convention Hall"); ViewBag.service = "Convention Hall"; }
            else if (servicetype == "BanquetHall")
            { ViewBag.records = vendorProductsService.Getvendorproducts_Result("Banquet Hall"); ViewBag.service = "Banquet Hall"; }
            else if (servicetype == "FunctionHall")
            { ViewBag.records = vendorProductsService.Getvendorproducts_Result("Function Hall"); ViewBag.service = "Function Hall"; }
            else
                ViewBag.records = vendorProductsService.Getvendorproducts_Result(servicetype);
            return View();
        }
    }
}