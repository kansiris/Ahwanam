using MaaAahwanam.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MaaAahwanam.Web.Areas.Admin.Controllers
{
    public class vendorController : Controller
    {
        VendorSetupService vendorSetupService = new VendorSetupService();

        // GET: Admin/vendor
        public ActionResult Index( string type)
        {
            //if (type == null) {
            //    ViewBag.VendorList = null;
            //}
            //else
            //{
            ViewBag.type = type;
            ViewBag.VendorList = vendorSetupService.AllVendorList(type);
            //}
            return View();
        }
    }
}