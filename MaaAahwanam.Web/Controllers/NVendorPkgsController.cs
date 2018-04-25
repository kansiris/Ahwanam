using MaaAahwanam.Models;
using MaaAahwanam.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MaaAahwanam.Web.Controllers
{
    public class NVendorPkgsController : Controller
    {
        VendorProductsService vendorProductsService = new VendorProductsService();

        // GET: NVendorPackages
        public ActionResult Index(string id )
        {
            var pkgs = vendorProductsService.getvendorpkgs(id);
            ViewBag.pacakagerecord = pkgs;
            ViewBag.id = id;
            return View();
        }
    }
}