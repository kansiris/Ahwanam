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
        UserLoginDetailsService userLoginDetailsService = new UserLoginDetailsService();
        // GET: AvailableServices
        public ActionResult Index(string id)
        {
            string[] services = { "Venue", "Catering", "Photography", "Decorator", "Other" };
            string vid = "";
            vendorMaster = vendorMasterService.GetVendor(long.Parse(id));
            if (vendorMaster.ServicType.Split(',').Contains("Venue"))
            {
                var vendorVenue = vendorVenueSignUpService.GetVendorVenue(long.Parse(id)).ToList();
                ViewBag.venueid = string.Join(",", vendorVenue.Select(m => m.Id));
                ViewBag.venuetype = string.Join(",", vendorVenue.Select(m => m.VenueType));
                ViewBag.venuescount = vendorVenue.Count();
                //vid = vid + "," + string.Join(",", vendorVenue.Select(m => m.Id));   //vid = vid + "," + vendorVenue.Id.ToString();
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
            if (vendorMaster.ServicType.Split(',').Contains("Decorator"))
            {
                VendorsDecorator vendorsDecorator = vendorVenueSignUpService.GetVendorDecorator(long.Parse(id));
                vid = vid + "," + vendorsDecorator.Id.ToString();
            }
            if (vendorMaster.ServicType.Split(',').Contains("Other"))
            {
                VendorsOther vendorsOther = vendorVenueSignUpService.GetVendorOther(long.Parse(id));
                vid = vid + "," + vendorsOther.Id.ToString();
            }
            //ViewBag.services = new { type = vendorMaster.ServicType.Split(','), vendorid = vid.TrimStart(',').Split(',') };
            ViewBag.services = services.Intersect(vendorMaster.ServicType.Split(',')).ToList();//vendorMaster.ServicType.Split(',');
            ViewBag.vid = vid.TrimStart(',');
            ViewBag.vendormasterid = id;
            return View();
        }

        [HttpPost]
        public ActionResult Index(string services, string id, string categories)
        {
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                if (services != "" && services != null)
                {
                    string[] venueservices = { "Convention Hall", "Function Hall", "Banquet Hall", "Meeting Room", "Open Lawn", "Roof Top", "Hotel", "Resort" };
                    string[] cateringservices = { "Indian", "Chinese", "Mexican", "South Indian", "Continental", "Multi Cuisine", "Chaat", "Fast Food", "Others" };
                    string[] photographyservices = { "Wedding", "Candid", "Portfolio", "Fashion", "Toddler", "Videography", "Conventional", "Cinematography", "Others" };
                    string[] decoratorservices = { "Florists", "TentHouse Decorators", "Others" };

                    List<string> matchingvenues = venueservices.Intersect(categories.Split(',')).ToList();
                    List<string> matchingcatering = cateringservices.Intersect(categories.Split(',')).ToList();
                    List<string> matchingphotography = photographyservices.Intersect(categories.Split(',')).ToList();
                    List<string> matchingdecorators = decoratorservices.Intersect(categories.Split(',')).ToList();

                    vendorMaster = vendorMasterService.GetVendor(long.Parse(id));
                    //vendorMaster.ServicType = string.Join(",", (services + "," + data.ServicType).Split(',').Distinct());
                    vendorMaster.ServicType = vendorMaster.ServicType + "," + services;
                    vendorMaster = vendorMasterService.UpdateVendorMaster(vendorMaster, long.Parse(id));

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
                    if (services.Split(',').Contains("Decorator"))
                    {
                        VendorsDecorator vendorsDecorator = new VendorsDecorator();
                        vendorsDecorator.VendorMasterId = vendorMaster.Id;
                        vendorsDecorator.DecorationType = string.Join<string>(",", matchingdecorators);
                        vendorsDecorator = vendorVenueSignUpService.AddVendorDecorator(vendorsDecorator);
                    }
                }
                return Content("<script language='javascript' type='text/javascript'>alert('New Service(s) Added Successfully');location.href='" + @Url.Action("Index", "AvailableServices", new { id = id }) + "'</script>");
            }
            else
            {
                return RedirectToAction("Index", "HomePage");
            }
        }

        public ActionResult changeid(string id)
        {
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                string email = userLoginDetailsService.Getusername(long.Parse(id));
                vendorMaster = vendorMasterService.GetVendorByEmail(email);
                //return View("AvailableServices", vendorMaster.Id);
                return RedirectToAction("Index", "AvailableServices", new { id = vendorMaster.Id });
            }
            return RedirectToAction("SignOut", "SampleStorefront");
        }
    }
}