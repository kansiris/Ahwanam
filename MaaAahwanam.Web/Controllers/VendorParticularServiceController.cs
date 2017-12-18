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
        VenorVenueSignUpService venorVenueSignUpService = new VenorVenueSignUpService();
        // GET: VendorParticularService
        public ActionResult Index(string type, string id, string vid)
        {
            if (type == "Conventions" || type == "Resorts" || type == "Hotels")
                type = "Venue";
            ViewBag.Productinfo = productInfoService.getProductsInfo_Result(int.Parse(id), type, int.Parse(vid)); //GetProductsInfo_Result Productinfo
            ViewBag.vendor = null;
            if (type == "Venue")
                ViewBag.Venue = venorVenueSignUpService.GetParticularVendorVenue(long.Parse(id), long.Parse(vid));
            else if (type == "Catering")
                ViewBag.Catering = venorVenueSignUpService.GetParticularVendorCatering(long.Parse(id), long.Parse(vid));
            else if (type == "Decorator")
                ViewBag.Decorator = venorVenueSignUpService.GetParticularVendorDecorator(long.Parse(id), long.Parse(vid));
            else if (type == "Photography")
                ViewBag.Photography = venorVenueSignUpService.GetParticularVendorPhotography(long.Parse(id), long.Parse(vid));
            else if (type == "Other")
                ViewBag.Other = venorVenueSignUpService.GetParticularVendorOther(long.Parse(id), long.Parse(vid));
            int imagescount = (ViewBag.Productinfo.image != null) ? ViewBag.Productinfo.image.Split(",").Count() : 0;
            ViewBag.image1 = (imagescount > 0) ? ViewBag.Productinfo.image.Split(",")[0] : null;
            ViewBag.image2 = (imagescount > 1) ? ViewBag.Productinfo.image.Split(",")[1] : null;
            ViewBag.image3 = (imagescount > 2) ? ViewBag.Productinfo.image.Split(",")[2] : null;
            return View();
        }
    }
}