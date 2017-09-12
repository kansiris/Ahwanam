using MaaAahwanam.Models;
using MaaAahwanam.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MaaAahwanam.Web.Controllers
{
    public class AvailableServicesController : Controller
    {
        VenorVenueSignUpService venorVenueSignUpService = new VenorVenueSignUpService();
        Vendormaster vendorMaster = new Vendormaster();
        VendorMasterService vendorMasterService = new VendorMasterService();
        // GET: AvailableServices
        public ActionResult Index(string id)
        {
            string vid = "";
            vendorMaster = vendorMasterService.GetVendor(long.Parse(id));
            if (vendorMaster.ServicType.Split(',').Contains("Venue"))
            {
                VendorVenue vendorVenue = venorVenueSignUpService.GetVendorVenue(long.Parse(id));
                vid = vid + ","+ vendorVenue.Id.ToString();
            }
            if (vendorMaster.ServicType.Split(',').Contains("Catering"))
            {
                VendorsCatering vendorsCatering = venorVenueSignUpService.GetVendorCatering(long.Parse(id));
                vid = vid + ","+ vendorsCatering.Id.ToString();
            }
            //ViewBag.services = new { type = vendorMaster.ServicType.Split(','), vendorid = vid.TrimStart(',').Split(',') };
            ViewBag.services = vendorMaster.ServicType.Split(',');
            ViewBag.vid = vid.TrimStart(',');
            return View();
        }
    }
}