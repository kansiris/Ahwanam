using MaaAahwanam.Models;
using MaaAahwanam.Repository;
using MaaAahwanam.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MaaAahwanam.Web.Controllers
{
    public class VendorParticularServiceController : Controller
    {
        ProductInfoService productInfoService = new ProductInfoService();
        VendorMasterService vendorMasterService = new VendorMasterService();
        VendorImageService vendorImageService = new VendorImageService();
        VenorVenueSignUpService venorVenueSignUpService = new VenorVenueSignUpService();
        // GET: VendorParticularService
        public ActionResult Index(string type, string id, string vid)
        {
            int imagescount = 0;
            if (type == "Conventions" || type == "Resorts" || type == "Hotels")
                type = "Venue";
            if (type == "Mehendi" || type == "Pandit")
                type = "Other";
            //var data = productInfoService.getProductsInfo_Result(int.Parse(id), type, int.Parse(vid)); //GetProductsInfo_Result Productinfo
            var data = vendorMasterService.GetVendor(long.Parse(id)); //GetProductsInfo_Result Productinfo
            var imageslist = vendorImageService.GetVendorAllImages(long.Parse(id));
            ViewBag.Productinfo = data;
            ViewBag.vendor = null;
            if (type == "Venue")
                ViewBag.Venue = venorVenueSignUpService.GetVendorVenue(long.Parse(id)); //, long.Parse(vid)
            else if (type == "Catering")
                ViewBag.Catering = venorVenueSignUpService.GetVendorCatering(long.Parse(id)); //, long.Parse(vid)
            else if (type == "Decorator")
                ViewBag.Decorator = venorVenueSignUpService.GetVendorDecorator(long.Parse(id)); //, long.Parse(vid)
            else if (type == "Photography")
                ViewBag.Photography = venorVenueSignUpService.GetVendorPhotography(long.Parse(id)); //, long.Parse(vid)
            else if (type == "Other")
                ViewBag.Other = venorVenueSignUpService.GetVendorOther(long.Parse(id)); //, long.Parse(vid)
            if (ViewBag.Productinfo != null)
            imagescount = (imageslist != null) ? imageslist.Count() : 0;
            ViewBag.image1 = (imagescount > 0) ? imageslist[0].Replace(" ","") : null;
            ViewBag.image2 = (imagescount > 1) ? imageslist[1].Replace(" ", "") : null;
            ViewBag.image3 = (imagescount > 2) ? imageslist[2].Replace(" ", "") : null;
            ViewBag.imagecount = imagescount - 3;
            return View();
        }
    }
}