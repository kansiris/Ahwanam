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
        VendorImageService vendorImageService = new VendorImageService();
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

        public ActionResult deleteservice(string id, string vid, string type)
        {
            int count = 0;
            if (type == "Venue")
                count = vendorVenueSignUpService.GetVendorVenue(long.Parse(id)).ToList().Count;
            if (type == "Catering")
                count = vendorVenueSignUpService.GetVendorCatering(long.Parse(id)).ToList().Count;
            if (type == "Photography")
                count = vendorVenueSignUpService.GetVendorPhotography(long.Parse(id)).Count;
            if (type == "EventManagement")
                count = vendorVenueSignUpService.GetVendorEventOrganiser(long.Parse(id)).Count;
            if (type == "Decorator")
                count = vendorVenueSignUpService.GetVendorDecorator(long.Parse(id)).Count;
            if (type == "Other")
                count = vendorVenueSignUpService.GetVendorOther(long.Parse(id)).Count;
            if (count > 1)
            {
                string msg = vendorVenueSignUpService.RemoveVendorService(vid, type);
                string message = vendorImageService.DeleteAllImages(long.Parse(id), long.Parse(vid));
                return Content("<script language='javascript' type='text/javascript'>alert('Service " + msg + "');location.href='/NVendorStoreFront/Index?id=" + id +"'</script>");
            }
            else
            {
                long value = vendorVenueSignUpService.UpdateVendorService(id, vid, type);
                string message = vendorImageService.DeleteAllImages(long.Parse(id), long.Parse(vid));
                if (value > 0)
                    return Content("<script language='javascript' type='text/javascript'>alert('Service Removed');location.href='/NVendorStoreFront/Index?id=" + id + "'</script>");
                else
                    return Content("<script language='javascript' type='text/javascript'>alert('Something Went Wrong!!! Try Again After Some Time');location.href='/NVendorStoreFront/Index?id=" + id + "'</script>");
            }
        }
    }
}