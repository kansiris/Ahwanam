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
            int imagescount = 0;
            if (type == "Conventions" || type == "Resorts" || type == "Hotels")
                type = "Venue";
            if (type == "Mehendi")
                type = "Other";
            var data = productInfoService.getProductsInfo_Result(int.Parse(id), type, int.Parse(vid)); //GetProductsInfo_Result Productinfo
            ViewBag.Productinfo = data;
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
            if(ViewBag.Productinfo != null)
            imagescount = (data.image != null) ? data.image.Split(',').Count() : 0;
            ViewBag.image1 = (imagescount > 0) ? data.image.Split(',')[0].Replace(" ","") : null;
            ViewBag.image2 = (imagescount > 1) ? data.image.Split(',')[1].Replace(" ", "") : null;
            ViewBag.image3 = (imagescount > 2) ? data.image.Split(',')[2].Replace(" ", "") : null;
            return View();
        }
    }
}