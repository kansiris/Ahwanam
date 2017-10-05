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
        VenorVenueSignUpService vendorVenueSignUpService = new VenorVenueSignUpService();
        Vendormaster vendorMaster = new Vendormaster();
        VendorMasterService vendorMasterService = new VendorMasterService();
        // GET: AvailableServices
        public ActionResult Index(string id)
        {
            string vid = "";
            vendorMaster = vendorMasterService.GetVendor(long.Parse(id));
            if (vendorMaster.ServicType.Split(',').Contains("Venue"))
            {
                VendorVenue vendorVenue = vendorVenueSignUpService.GetVendorVenue(long.Parse(id));
                vid = vid + "," + vendorVenue.Id.ToString();
            }
            if (vendorMaster.ServicType.Split(',').Contains("Catering"))
            {
                VendorsCatering vendorsCatering = vendorVenueSignUpService.GetVendorCatering(long.Parse(id));
                vid = vid + "," + vendorsCatering.Id.ToString();
            }
            if (vendorMaster.ServicType.Split(',').Contains("Photography"))
            {
                VendorsPhotography vendorsPhotography = vendorVenueSignUpService.GetVendorPhotography(long.Parse(id));
                vid = vid + "," + vendorsPhotography.Id.ToString();
            }
            //ViewBag.services = new { type = vendorMaster.ServicType.Split(','), vendorid = vid.TrimStart(',').Split(',') };
            ViewBag.services = vendorMaster.ServicType.Split(',');
            ViewBag.vid = vid.TrimStart(',');
            return View();
        }

        [HttpPost]
        public ActionResult Index(string services, string id, string categories)
        {
            if (services != "" && services != null)
            {
                string[] venueservices = { "Convention Hall", "Function Hall", "Banquet Hall", "Meeting Room", "Open Lawn", "Roof Top", "Hotel", "Resort" };
                string[] cateringservices = { "Indian", "Chinese", "Mexican", "South Indian", "Continental", "Multi Cuisine", "Chaat", "Fast Food", "Others" };
                string[] photographyservices = { "Wedding", "Candid", "Portfolio", "Fashion", "Toddler", "Videography", "Conventional", "Cinematography", "Others" };
                List<string> matchingvenues = venueservices.Intersect(categories.Split(',')).ToList();
                List<string> matchingcatering = cateringservices.Intersect(categories.Split(',')).ToList();
                List<string> matchingphotography = photographyservices.Intersect(categories.Split(',')).ToList();

                if (services.Split(',').Contains("Venue"))
                {
                    VendorVenue vendorVenue = new VendorVenue();
                    vendorVenue.VenueType = string.Join<string>(",", matchingvenues);
                    vendorVenue.VendorMasterId = long.Parse(id);
                    vendorVenue = vendorVenueSignUpService.AddVendorVenue(vendorVenue);
                }
                if (services.Split(',').Contains("Catering"))
                {
                    VendorsCatering vendorsCatering = new VendorsCatering();
                    vendorsCatering.VendorMasterId = long.Parse(id);
                    vendorsCatering.CuisineType = string.Join<string>(",", matchingcatering);
                    vendorsCatering = vendorVenueSignUpService.AddVendorCatering(vendorsCatering);
                }
                if (services.Split(',').Contains("Photography"))
                {
                    VendorsPhotography vendorsPhotography = new VendorsPhotography();
                    vendorsPhotography.VendorMasterId = long.Parse(id);
                    vendorsPhotography.PhotographyType = string.Join<string>(",", matchingphotography);
                    vendorsPhotography = vendorVenueSignUpService.AddVendorPhotography(vendorsPhotography);
                }
            }
            return Content("<script language='javascript' type='text/javascript'>alert('General Information Registered Successfully');location.href='" + @Url.Action("Index", "AvailableServices", new { id = id }) + "'</script>");
        }
    }
}