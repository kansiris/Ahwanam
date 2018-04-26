using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaaAahwanam.Web.Custom;
using MaaAahwanam.Service;
using MaaAahwanam.Models;

namespace MaaAahwanam.Web.Controllers
{
    public class NVendorManageStoreFrontController : Controller
    {
        VenorVenueSignUpService vendorVenueSignUpService = new VenorVenueSignUpService();
        VendorMasterService vendorMasterService = new VendorMasterService();
        // GET: NVendorManageStoreFront
        public ActionResult Index(string id, string vid, string category, string subcategory)
        {
            ViewBag.id = id;
            ViewBag.Vendor = vendorMasterService.GetVendor(long.Parse(id));
            ViewBag.display = "0";
            if (vid != "" && vid != null)
            {
                ViewBag.display = "1";
                if (category == "Venue")
                {
                    VendorVenueService vendorVenueService = new VendorVenueService();
                    ViewBag.service = vendorVenueService.GetVendorVenue(long.Parse(id), long.Parse(vid));
                    ViewBag.categorytype = ViewBag.service.VenueType;
                    return View();
                }
                if (category == "Catering")
                {
                    VendorCateringService vendorCateringService = new VendorCateringService();
                    ViewBag.service = vendorCateringService.GetVendorCatering(long.Parse(id), long.Parse(vid));
                    ViewBag.categorytype = ViewBag.service.CuisineType;
                    return View();
                }
                if (category == "Photography")
                {
                    VendorPhotographyService vendorPhotographyService = new VendorPhotographyService();
                    ViewBag.service = vendorPhotographyService.GetVendorPhotography(long.Parse(id), long.Parse(vid));
                    ViewBag.categorytype = ViewBag.service.PhotographyType;
                    return View();
                }
                if (category == "Decorator")
                {
                    VendorDecoratorService vendorDecoratorService = new VendorDecoratorService();
                    ViewBag.service = vendorDecoratorService.GetVendorDecorator(long.Parse(id), long.Parse(vid));
                    ViewBag.categorytype = ViewBag.service.DecorationType;
                    return View();
                }
                if (category == "Other")
                {
                    VendorOthersService vendorOthersService = new VendorOthersService();
                    ViewBag.service = vendorOthersService.GetVendorOther(long.Parse(id), long.Parse(vid));
                    ViewBag.categorytype = ViewBag.service.type;
                    return View();
                }
            }
            return View();
        }

        [HttpPost]
        public ActionResult Index(string id, string command, string serviceselection, string subcategory,string vid)
        {
            long count = addservice(serviceselection, subcategory, long.Parse(id));
            string msg = "";
            if (count > 0)  msg = "Service Added Successfully";
            else msg = "Failed To Add Sevice";
            return Content("<script language='javascript' type='text/javascript'>alert('" + msg + "');location.href='/NVendorManageStoreFront/Index?id=" + id + "&&vid=" + count + "&&category=" + serviceselection + "&&subcategory=" + subcategory + "'</script>");
        }

        public JsonResult filtercategories(string type)
        {
            string venueservices = "Select Sub-Category,Convention Hall,Function Hall,Banquet Hall,Meeting Room,Open Lawn,Roof Top,Hotel,Resort";
            string cateringservices = "Select Sub-Category,Indian,Chinese,Mexican,South Indian,Continental,Multi Cuisine,Chaat,Fast Food,Others";
            string photographyservices = "Select Sub-Category,Wedding,Candid,Portfolio,Fashion,Toddler,Videography,Conventional,Cinematography,Others";
            //string eventservices = "Select Sub-Category,Corporate Events,Brand Promotion,Fashion Shows,Exhibition,Conference & Seminar,Wedding Management,Birthday Planning & Celebrations,Live Concerts,Musical Nights,Celebrity Shows";
            string decoratorservices = "Select Sub-Category,Florists,TentHouse Decorators,Others";
            string otherservices = "Select Sub-Category,Mehendi,Pandit";
            if (type == "Venue") return Json(venueservices, JsonRequestBehavior.AllowGet);
            else if (type == "Catering") return Json(cateringservices, JsonRequestBehavior.AllowGet);
            else if (type == "Photography") return Json(photographyservices, JsonRequestBehavior.AllowGet);
            else if (type == "Decorator") return Json(decoratorservices, JsonRequestBehavior.AllowGet);
            else if (type == "Other") return Json(otherservices, JsonRequestBehavior.AllowGet);
            return Json("Fail", JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateStoreFront(string command, string category, string subcategory, string id, string vid,Vendormaster vendormaster, VendorVenue vendorVenue)
        {
            if (command == "one")
            {
                vendormaster = vendorMasterService.UpdateVendorMaster(vendormaster, long.Parse(id)); //updating Vendor Master
                return Json("Basic Details Updated");
            }
            else if (command == "two")
            {

            }
            else if (command == "three")
            {
                if (category == "Venue")
                {
                    var venuedata = vendorVenue;
                    vendorVenue = vendorVenueSignUpService.GetParticularVendorVenue(long.Parse(id), long.Parse(vid)); // Retrieving Particular Vendor Record
                    vendorVenue.Description = venuedata.Description;
                    vendorVenue.Dimentions = venuedata.Dimentions;
                    vendorVenue.Minimumseatingcapacity = venuedata.Minimumseatingcapacity;
                    vendorVenue.Maximumcapacity = venuedata.Maximumcapacity;
                    vendorVenue.name = venuedata.name;
                    vendorVenue = vendorVenueSignUpService.UpdateVenue(vendorVenue, vendormaster, long.Parse(id), long.Parse(vid));
                    return Json("Hall Details Updated");
                }
            }
            else if (command == "four")
            {
                var venuedata = vendorVenue;
                if (category == "Venue")
                {
                    vendorVenue = vendorVenueSignUpService.GetParticularVendorVenue(long.Parse(id), long.Parse(vid)); // Retrieving Particular Vendor Record
                    vendorVenue.Address = venuedata.Address;
                    vendorVenue.City = venuedata.City;
                    vendorVenue.State = venuedata.State;
                    vendorVenue.Landmark = venuedata.Landmark;
                    vendorVenue.ZipCode = venuedata.ZipCode;
                    vendorVenue.GeoLocation = venuedata.GeoLocation;
                    vendorVenue = vendorVenueSignUpService.UpdateVenue(vendorVenue, vendormaster, long.Parse(id), long.Parse(vid));
                }
                else if (category == "Catering")
                {
                    VendorCateringService vendorCateringService = new VendorCateringService();
                    VendorsCatering vendorsCatering = vendorVenueSignUpService.GetParticularVendorCatering(long.Parse(id), long.Parse(vid));
                    vendorsCatering.Address = vendorVenue.Address;
                    vendorsCatering.City = vendorVenue.City;
                    vendorsCatering.State = vendorVenue.State;
                    vendorsCatering.Landmark = vendorVenue.Landmark;
                    vendorsCatering.ZipCode = vendorVenue.ZipCode;
                    vendorsCatering = vendorVenueSignUpService.UpdateCatering(vendorsCatering, vendormaster, long.Parse(id), long.Parse(vid));
                }
                else if (category == "Photography")
                {
                    VendorsPhotography vendorsPhotography = vendorVenueSignUpService.GetParticularVendorPhotography(long.Parse(id), long.Parse(vid));
                    vendorsPhotography.Address = vendorVenue.Address;
                    vendorsPhotography.City = vendorVenue.City;
                    vendorsPhotography.State = vendorVenue.State;
                    vendorsPhotography.Landmark = vendorVenue.Landmark;
                    vendorsPhotography.ZipCode = vendorVenue.ZipCode;
                    vendorsPhotography = vendorVenueSignUpService.UpdatePhotography(vendorsPhotography, vendormaster, long.Parse(id), long.Parse(vid));
                }
                else if (category == "Decorator")
                {
                    VendorsDecorator vendorsDecorator = vendorVenueSignUpService.GetParticularVendorDecorator(long.Parse(id), long.Parse(vid));
                    vendorsDecorator.Address = vendorVenue.Address;
                    vendorsDecorator.City = vendorVenue.City;
                    vendorsDecorator.State = vendorVenue.State;
                    vendorsDecorator.Landmark = vendorVenue.Landmark;
                    vendorsDecorator.ZipCode = vendorVenue.ZipCode;
                    vendorsDecorator = vendorVenueSignUpService.UpdateDecorator(vendorsDecorator, vendormaster, long.Parse(id), long.Parse(vid));
                }
                else if (category == "Other")
                {
                    VendorsOther vendorsOther = vendorVenueSignUpService.GetParticularVendorOther(long.Parse(id), long.Parse(vid));
                    vendorsOther.Address = vendorVenue.Address;
                    vendorsOther.City = vendorVenue.City;
                    vendorsOther.State = vendorVenue.State;
                    vendorsOther.Landmark = vendorVenue.Landmark;
                    vendorsOther.ZipCode = vendorVenue.ZipCode;
                    vendorsOther = vendorVenueSignUpService.UpdateOther(vendorsOther, vendormaster, long.Parse(id), long.Parse(vid));
                }
                return Json("Address Updated");
            }
            return Json(JsonRequestBehavior.AllowGet);
        }

        public long addservice(string category, string subcategory, long id)
        {
            long count = 0;
            if (category == "Venue")
            {
                VendorVenue vendorVenue = new VendorVenue();
                vendorVenue.VendorMasterId = id;
                vendorVenue.VenueType = subcategory;
                vendorVenue = vendorVenueSignUpService.AddVendorVenue(vendorVenue);
                if (vendorVenue.Id != 0) count = vendorVenue.Id; 
            }
            else if (category == "Catering")
            {
                VendorsCatering vendorsCatering = new VendorsCatering();
                vendorsCatering.VendorMasterId = id;
                vendorsCatering.CuisineType = subcategory;
                vendorsCatering = vendorVenueSignUpService.AddVendorCatering(vendorsCatering);
                if (vendorsCatering.Id != 0) count = vendorsCatering.Id;
            }
            else if (category == "Photography")
            {
                VendorsPhotography vendorsPhotography = new VendorsPhotography();
                vendorsPhotography.VendorMasterId = id;
                vendorsPhotography.PhotographyType = subcategory;
                vendorsPhotography = vendorVenueSignUpService.AddVendorPhotography(vendorsPhotography);
                if (vendorsPhotography.Id != 0) count = vendorsPhotography.Id;
            }
            else if (category == "Decorator")
            {
                VendorsDecorator vendorsDecorator = new VendorsDecorator();
                vendorsDecorator.VendorMasterId = id;
                vendorsDecorator.DecorationType = subcategory;
                vendorsDecorator = vendorVenueSignUpService.AddVendorDecorator(vendorsDecorator);
                if (vendorsDecorator.Id != 0) count = vendorsDecorator.Id;
            }
            //if (category == "EventManagement")
            //{
            //    VendorsEventOrganiser vendorsEventOrganiser = new VendorsEventOrganiser();
            //    vendorsEventOrganiser.VendorMasterId = id;
            //    vendorsEventOrganiser = vendorVenueSignUpService.AddVendorEventOrganiser(vendorsEventOrganiser);
            //    if (vendorsEventOrganiser.Id != 0) count = vendorsEventOrganiser.Id;
            //}
            else if (category == "Other")
            {
                VendorsOther vendorsOther = new VendorsOther();
                vendorsOther.VendorMasterId = id;
                vendorsOther.MinOrder = "0";
                vendorsOther.MaxOrder = "0";
                vendorsOther.Status = "InActive";
                vendorsOther.UpdatedBy = 2;
                vendorsOther.UpdatedDate = Convert.ToDateTime(DateTime.UtcNow.ToShortDateString());
                vendorsOther.type = subcategory;
                vendorsOther = vendorVenueSignUpService.AddVendorOther(vendorsOther);
                if (vendorsOther.Id != 0) count = vendorsOther.Id;
            }
            return count;
        }

        public string updaterecord()
        {
            return "";
        }
    }
}