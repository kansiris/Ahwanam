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
        const string imagepath = @"/vendorimages/";
        VendorImageService vendorImageService = new VendorImageService();
        // GET: AvailableServices
        public ActionResult Index(string id)
        {
            string[] services = { "Venue", "Catering", "Photography", "Event Management", "Decorator", "Other" };
            string vid = "";
            vendorMaster = vendorMasterService.GetVendor(long.Parse(id));
            if (vendorMaster.ServicType.Split(',').Contains("Venue"))
                ViewBag.venuerecord = vendorVenueSignUpService.GetVendorVenue(long.Parse(id)).ToList();
            if (vendorMaster.ServicType.Split(',').Contains("Catering"))
                ViewBag.cateringrecord = vendorVenueSignUpService.GetVendorCatering(long.Parse(id)).ToList();
            if (vendorMaster.ServicType.Split(',').Contains("Photography"))
                ViewBag.Photographyrecord = vendorVenueSignUpService.GetVendorPhotography(long.Parse(id));
            if (vendorMaster.ServicType.Split(',').Contains("Event Management"))
                ViewBag.Eventrecord = vendorVenueSignUpService.GetVendorEventOrganiser(long.Parse(id));
            if (vendorMaster.ServicType.Split(',').Contains("Decorator"))
                ViewBag.Decoratorrecord = vendorVenueSignUpService.GetVendorDecorator(long.Parse(id));
            if (vendorMaster.ServicType.Split(',').Contains("Other"))
                ViewBag.Otherrecord = vendorVenueSignUpService.GetVendorOther(long.Parse(id));
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
                    string[] otherservices = { "Mehendi" };

                    List<string> matchingvenues = venueservices.Intersect(categories.Split(',')).ToList();
                    List<string> matchingcatering = cateringservices.Intersect(categories.Split(',')).ToList();
                    List<string> matchingphotography = photographyservices.Intersect(categories.Split(',')).ToList();
                    List<string> matchingdecorators = decoratorservices.Intersect(categories.Split(',')).ToList();
                    List<string> matchingothers = otherservices.Intersect(categories.Split(',')).ToList();

                    vendorMaster = vendorMasterService.GetVendor(long.Parse(id));
                    vendorMaster.ServicType = string.Join(",", (services + "," + vendorMaster.ServicType).Split(',').Distinct());
                    //vendorMaster.ServicType = vendorMaster.ServicType + "," + services;
                    vendorMaster = vendorMasterService.UpdateVendorMaster(vendorMaster, long.Parse(id));

                    if (services.Split(',').Contains("Venue"))
                    {
                        VendorVenue vendorVenue = new VendorVenue();
                        //vendorVenue.VenueType = string.Join<string>(",", matchingvenues);
                        //vendorVenue.VendorMasterId = long.Parse(id);
                        //vendorVenue = vendorVenueSignUpService.AddVendorVenue(vendorVenue);
                        for (int a = 0; a < matchingvenues.Count(); a++)
                        {
                            vendorVenue.VendorMasterId = long.Parse(id);
                            vendorVenue.VenueType = matchingvenues[a];
                            vendorVenue = vendorVenueSignUpService.AddVendorVenue(vendorVenue);
                        }
                    }
                    if (services.Split(',').Contains("Catering"))
                    {
                        VendorsCatering vendorsCatering = new VendorsCatering();
                        
                        //vendorsCatering.CuisineType = string.Join<string>(",", matchingcatering);
                        //vendorsCatering = vendorVenueSignUpService.AddVendorCatering(vendorsCatering);
                        for (int a = 0; a < matchingcatering.Count(); a++)
                        {
                            vendorsCatering.VendorMasterId = long.Parse(id);
                            vendorsCatering.CuisineType = matchingcatering[a];
                            vendorsCatering = vendorVenueSignUpService.AddVendorCatering(vendorsCatering);
                        }
                    }
                    if (services.Split(',').Contains("Photography"))
                    {
                        VendorsPhotography vendorsPhotography = new VendorsPhotography();
                        //vendorsPhotography.VendorMasterId = long.Parse(id);
                        //vendorsPhotography.PhotographyType = string.Join<string>(",", matchingphotography);
                        //vendorsPhotography = vendorVenueSignUpService.AddVendorPhotography(vendorsPhotography);
                        for (int a = 0; a < matchingphotography.Count(); a++)
                        {
                            vendorsPhotography.VendorMasterId = long.Parse(id);
                            vendorsPhotography.PhotographyType = matchingphotography[a];
                            vendorsPhotography = vendorVenueSignUpService.AddVendorPhotography(vendorsPhotography);
                        }
                    }
                    if (services.Split(',').Contains("Decorator"))
                    {
                        VendorsDecorator vendorsDecorator = new VendorsDecorator();
                        //vendorsDecorator.VendorMasterId = vendorMaster.Id;
                        //vendorsDecorator.DecorationType = string.Join<string>(",", matchingdecorators);
                        //vendorsDecorator = vendorVenueSignUpService.AddVendorDecorator(vendorsDecorator);
                        for (int a = 0; a < matchingdecorators.Count(); a++)
                        {
                            vendorsDecorator.VendorMasterId = long.Parse(id);
                            vendorsDecorator.DecorationType = matchingdecorators[a];
                            vendorsDecorator = vendorVenueSignUpService.AddVendorDecorator(vendorsDecorator);
                        }
                    }
                    if (services.Split(',').Contains("Other"))
                    {
                        VendorsOther vendorsOther = new VendorsOther();
                        vendorsOther.MinOrder = "0.0";
                        vendorsOther.MaxOrder = "0.0";
                        vendorsOther.ItemCost = 0;
                        vendorsOther.UpdatedBy = 0;
                        //vendorsDecorator.VendorMasterId = vendorMaster.Id;
                        //vendorsDecorator.DecorationType = string.Join<string>(",", matchingdecorators);
                        //vendorsDecorator = vendorVenueSignUpService.AddVendorDecorator(vendorsDecorator);
                        for (int a = 0; a < matchingothers.Count(); a++)
                        {
                            vendorsOther.VendorMasterId = long.Parse(id);
                            vendorsOther.type = matchingothers[a];
                            vendorsOther = vendorVenueSignUpService.AddVendorOther(vendorsOther);
                        }
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
            return RedirectToAction("SignOut", "UserRegistration");
        }

        [HttpPost]
        public ActionResult UpdateServiceLogo(HttpPostedFileBase helpSectionImages, string id,string vid, string type)
        {
            string fileName = string.Empty;
            VendorImage vendorImage = new VendorImage();
            Vendormaster vendorMaster = new Vendormaster();
            vendorMaster.Id = long.Parse(id);
            vendorImage.VendorId = long.Parse(vid);
            if (helpSectionImages != null)
            {
                string path = System.IO.Path.GetExtension(helpSectionImages.FileName);
                var file1 = Request.Files[0];
                if (file1 != null && file1.ContentLength > 0)
                {
                    var filename = type + "_" + id + "_" + vid + "_" + "Logo" + path;
                    fileName = System.IO.Path.Combine(System.Web.HttpContext.Current.Server.MapPath(imagepath + filename));
                    file1.SaveAs(fileName);
                    vendorImage.ImageName = filename;
                    vendorImage = vendorImageService.AddVendorImage(vendorImage, vendorMaster);
                    return Json(vendorImage.ImageName);
                }
            }
            return Json(JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult deleteservice(string id, string vid, string type)
        {
            int count = 0;
            if (type =="Venue")
                count = vendorVenueSignUpService.GetVendorVenue(long.Parse(id)).ToList().Count;
            if (type == "Catering")
                count = vendorVenueSignUpService.GetVendorCatering(long.Parse(id)).ToList().Count;
            if (type == "Photography")
                count = vendorVenueSignUpService.GetVendorPhotography(long.Parse(id)).Count;
            if (type == "Event")
                count = vendorVenueSignUpService.GetVendorEventOrganiser(long.Parse(id)).Count;
            if (type == "Decorator")
                count = vendorVenueSignUpService.GetVendorDecorator(long.Parse(id)).Count;
            if (type == "Other")
                count = vendorVenueSignUpService.GetVendorOther(long.Parse(id)).Count;
            if (count > 1)
            {
                string msg = vendorVenueSignUpService.RemoveVendorService(vid,type);
                string message = vendorImageService.DeleteAllImages(long.Parse(id), long.Parse(vid));
                return Json(msg);
            }
            else
            {
                long value = vendorVenueSignUpService.UpdateVendorService(id, vid, type);
                string message = vendorImageService.DeleteAllImages(long.Parse(id), long.Parse(vid));
                if (value > 0)
                    return Json("Removed");
                else
                    return Json("Failed!!!");
            }
        }
    }
}