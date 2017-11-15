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
            ViewBag.records = vendorProductsService.Getvendorproducts_Result(service);
            return View();
        }
    }
}