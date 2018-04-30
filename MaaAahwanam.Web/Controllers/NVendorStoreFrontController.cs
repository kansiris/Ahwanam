using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaaAahwanam.Models;
using MaaAahwanam.Service;

namespace MaaAahwanam.Web.Controllers
{
    public class NVendorStoreFrontController : Controller
    {
        VenorVenueSignUpService vendorVenueSignUpService = new VenorVenueSignUpService();
        // GET: NVendorStoreFront
        public ActionResult Index(string id)
        {
            ViewBag.id = id;
            var venues = vendorVenueSignUpService.GetVendorVenue(long.Parse(id)).ToList();
            var catering = vendorVenueSignUpService.GetVendorCatering(long.Parse(id)).ToList();
            var photography = vendorVenueSignUpService.GetVendorPhotography(long.Parse(id));
            var decorators = vendorVenueSignUpService.GetVendorDecorator(long.Parse(id));
            var others = vendorVenueSignUpService.GetVendorOther(long.Parse(id));
            ViewBag.venues = venues;
            ViewBag.catering = catering;
            ViewBag.photography = photography;
            ViewBag.decorators = decorators;
            ViewBag.others = others;
            return View();
        }
    }
}