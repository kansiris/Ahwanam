using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaaAahwanam.Service;

namespace MaaAahwanam.Web.Controllers
{
    public class NParticularVendorController : Controller
    {
        ProductInfoService productInfoService = new ProductInfoService();
        VendorMasterService vendorMasterService = new VendorMasterService();
        VendorImageService vendorImageService = new VendorImageService();
        VenorVenueSignUpService venorVenueSignUpService = new VenorVenueSignUpService();
        // GET: NParticularVendor
        public ActionResult Index(string type, string id, string vid)
        {
            if (type == "Conventions" || type == "Resorts" || type == "Hotels")
                type = "Venue";
            if (type == "Mehendi" || type == "Pandit")
                type = "Other";
            //var data = productInfoService.getProductsInfo_Result(int.Parse(id), type, int.Parse(vid));
            var data = vendorMasterService.GetVendor(long.Parse(id)); //
            var allimages = vendorImageService.GetVendorAllImages(long.Parse(id));
            ViewBag.image = allimages.FirstOrDefault().Replace(" ", "");
            ViewBag.allimages = allimages;
            ViewBag.Productinfo = data;
            ViewBag.type = type;
            //if (type == "Venue")
            //    ViewBag.Venue = venorVenueSignUpService.GetVendorVenue(long.Parse(id)); //, long.Parse(vid)
            //else if (type == "Catering")
            //    ViewBag.Catering = venorVenueSignUpService.GetVendorCatering(long.Parse(id)); //, long.Parse(vid)
            //else if (type == "Decorator")
            //    ViewBag.Decorator = venorVenueSignUpService.GetVendorDecorator(long.Parse(id)); //, long.Parse(vid)
            //else if (type == "Photography")
            //    ViewBag.Photography = venorVenueSignUpService.GetVendorPhotography(long.Parse(id)); //, long.Parse(vid)
            //else if (type == "Other")
            //    ViewBag.Other = venorVenueSignUpService.GetVendorOther(long.Parse(id)); //, long.Parse(vid)
            return View();
        }

        public JsonResult WhishList(string type, string id, string vid,string command)
        {
            return Json(JsonRequestBehavior.AllowGet);
        }
    }
}