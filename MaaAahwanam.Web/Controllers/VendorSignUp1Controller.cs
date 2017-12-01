using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaaAahwanam.Models;
using MaaAahwanam.Service;

namespace MaaAahwanam.Web.Controllers
{
    public class VendorSignUp1Controller : Controller
    {
        VenorVenueSignUpService vendorVenueSignUpService = new VenorVenueSignUpService();
        VendorMasterService vendorMasterService = new VendorMasterService();

        // GET: VendorSignUp1
        public ActionResult Index(string id, string vid, string type)
        {
            ViewBag.data = vendorMasterService.GetVendor(long.Parse(id));
            ViewBag.type = type;
            ViewBag.country = new SelectList(CountryList(), "Value", "Text");
            if (type == "Venue")
            {
                VendorVenueService vendorVenueService = new VendorVenueService();
                ViewBag.service = vendorVenueService.GetVendorVenue(long.Parse(id), long.Parse(vid)).VenueType;
                return View();
            }
            if (type == "Catering")
            {
                VendorCateringService vendorCateringService = new VendorCateringService();
                ViewBag.service = vendorCateringService.GetVendorCatering(long.Parse(id), long.Parse(vid)).CuisineType;
                return View();
            }
            if (type == "Photography")
            {
                VendorPhotographyService vendorPhotographyService = new VendorPhotographyService();
                ViewBag.service = vendorPhotographyService.GetVendorPhotography(long.Parse(id), long.Parse(vid)).PhotographyType;
                return View();
            }
            if (type == "Decorator")
            {
                VendorDecoratorService vendorDecoratorService = new VendorDecoratorService();
                ViewBag.service = vendorDecoratorService.GetVendorDecorator(long.Parse(id), long.Parse(vid)).DecorationType;
                return View();
            }
            return View();
        }
        [HttpPost]
        public ActionResult Index([Bind(Prefix = "Item1")] Vendormaster vendorMaster, [Bind(Prefix = "Item2")] UserLogin userLogin, [Bind(Prefix = "Item3")]UserDetail userDetail, [Bind(Prefix = "Item4")]VendorVenue vendorVenue, string id, string vid, string type)
        {
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                string[] venueservices = { "Convention Hall", "Function Hall", "Banquet Hall", "Meeting Room", "Open Lawn", "Roof Top", "Hotel", "Resort" };
                string[] cateringservices = { "Indian", "Chinese", "Mexican", "South Indian", "Continental", "Multi Cuisine", "Chaat", "Fast Food", "Others" };
                string[] photographyservices = { "Wedding", "Candid", "Portfolio", "Fashion", "Toddler", "Videography", "Conventional", "Cinematography", "Others" };
                string[] decoratorservices = { "Florists", "TentHouse Decorators", "Others" };
                List<string> matchingvenues = null; List<string> matchingcatering = null; List<string> matchingphotography = null; List<string> matchingdecorators = null;
                if (vendorVenue.VenueType != null)
                {
                    if (type == "Venue") //if (vendorMaster.ServicType.Split(',').Contains("Venue"))
                        matchingvenues = venueservices.Intersect(vendorVenue.VenueType.Split(',')).ToList();
                    if (type == "Catering") //if (vendorMaster.ServicType.Split(',').Contains("Catering"))
                        matchingcatering = cateringservices.Intersect(vendorVenue.VenueType.Split(',')).ToList();
                    if (type == "Photography") //if (vendorMaster.ServicType.Split(',').Contains("Photography"))
                        matchingphotography = photographyservices.Intersect(vendorVenue.VenueType.Split(',')).ToList();
                    if (type == "Decorator") //if (vendorMaster.ServicType.Split(',').Contains("Decorator"))
                        matchingdecorators = decoratorservices.Intersect(vendorVenue.VenueType.Split(',')).ToList();
                }
                else
                    return Content("<script language='javascript' type='text/javascript'>alert('Select Atleat One Sub Category');location.href='/VendorSignUp1/Index?id=" + id + "&&vid=" + vid + "&&type=" + type + "'</script>");
                //userLogin = vendorVenueSignUpService.AddUserLogin(userLogin);
                //userDetail.UserLoginId = userLogin.UserLoginId;
                //userDetail = vendorVenueSignUpService.AddUserDetail(userDetail, vendorMaster);
                //vendorMaster = vendorVenueSignUpService.AddvendorMaster(vendorMaster);
                vendorMaster = vendorMasterService.UpdateVendorMaster(vendorMaster, long.Parse(id));
                if (matchingvenues != null)  //if (vendorMaster.ServicType.Split(',').Contains("Venue"))
                {
                    VendorVenueService vendorVenueService = new VendorVenueService();
                    vendorVenue = vendorVenueSignUpService.GetParticularVendorVenue(long.Parse(id), long.Parse(vid));
                    //vendorVenue.VendorMasterId = vendorMaster.Id;
                    //for (int i = 0; i < matchingvenues.Count; i++)
                    //{
                    var subtype = vendorVenueService.GetVendorVenue(long.Parse(id), long.Parse(vid));
                    vendorVenue.VenueType = string.Join<string>(",", matchingvenues);
                    if (subtype == null)
                        vendorVenue = vendorVenueSignUpService.AddVendorVenue(vendorVenue);
                    else
                        vendorVenue = vendorVenueSignUpService.UpdateVenue(vendorVenue, vendorMaster, long.Parse(id), long.Parse(vid));
                    //}
                    //vendorVenue.VenueType = string.Join<string>(",", matchingvenues);
                    //vendorVenue = vendorVenueSignUpService.AddVendorVenue(vendorVenue);
                    //vendorVenue = vendorVenueSignUpService.UpdateVenue(vendorVenue,vendorMaster,long.Parse(id), long.Parse(vid));
                }
                if (matchingcatering != null)  //if (vendorMaster.ServicType.Split(',').Contains("Catering"))
                {
                    VendorCateringService vendorCateringService = new VendorCateringService();
                    //VendorsCatering vendorsCatering = vendorVenueSignUpService.GetParticularVendorCatering(long.Parse(id), long.Parse(vid));
                    //string subcategories = vendorsCatering.CuisineType;
                    //vendorsCatering.VendorMasterId = vendorMaster.Id;
                    //for (int i = 0; i < matchingcatering.Count; i++)
                    //{
                    //var subtype = vendorCateringService.GetVendorCatering(long.Parse(id), long.Parse(vid));
                    //vendorsCatering.CuisineType = string.Join<string>(",", matchingcatering);
                    VendorsCatering vendorsCatering = new VendorsCatering();
                    vendorsCatering.VendorMasterId = vendorMaster.Id;
                    for (int a = 0; a < matchingcatering.Count(); a++)
                    {
                        vendorsCatering.CuisineType = matchingcatering[a];
                        vendorsCatering = vendorVenueSignUpService.AddVendorCatering(vendorsCatering);
                    }

                    #region working code

                    //if (vendorsCatering == null)
                    //    vendorsCatering = vendorVenueSignUpService.AddVendorCatering(vendorsCatering);
                    //else
                    //{
                    //    for (int a = 0; a < matchingcatering.Count; a++)
                    //    {
                    //        vendorsCatering.CuisineType = subcategories.Split(',')[a];
                    //        if (a == 0) //subtype.CuisineType.Split(',')[a]
                    //            vendorsCatering = vendorVenueSignUpService.UpdateCatering(vendorsCatering, vendorMaster, long.Parse(id), long.Parse(vid));
                    //        else
                    //        {
                    //            vendorsCatering = new VendorsCatering();
                    //            vendorsCatering.VendorMasterId = vendorMaster.Id;
                    //            vendorsCatering.CuisineType = subcategories.Split(',')[a];
                    //            vendorsCatering = vendorVenueSignUpService.AddVendorCatering(vendorsCatering);
                    //        }
                    //    }

                    #endregion

                    //vendorsCatering = vendorVenueSignUpService.UpdateCatering(vendorsCatering, vendorMaster, long.Parse(id), long.Parse(vid));
                    //}
                    //}
                    //vendorsCatering.CuisineType = string.Join<string>(",", matchingcatering);
                    //vendorsCatering = vendorVenueSignUpService.UpdateCatering(vendorsCatering, vendorMaster, long.Parse(id), long.Parse(vid));
                }
                if (matchingphotography != null)  //if (vendorMaster.ServicType.Split(',').Contains("Photography"))
                {
                    VendorPhotographyService vendorPhotographyService = new VendorPhotographyService();
                    VendorsPhotography vendorsPhotography = vendorVenueSignUpService.GetParticularVendorPhotography(long.Parse(id), long.Parse(vid));
                    vendorsPhotography.VendorMasterId = vendorMaster.Id;
                    //for (int i = 0; i < matchingphotography.Count; i++)
                    //{
                    var subtype = vendorPhotographyService.GetVendorPhotography(long.Parse(id), long.Parse(vid));
                    vendorsPhotography.PhotographyType = string.Join<string>(",", matchingphotography);
                    if (subtype == null)
                        vendorsPhotography = vendorVenueSignUpService.AddVendorPhotography(vendorsPhotography);
                    else
                        vendorsPhotography = vendorVenueSignUpService.UpdatePhotography(vendorsPhotography, vendorMaster, long.Parse(id), long.Parse(vid));
                    //}
                    //vendorsPhotography.PhotographyType = string.Join<string>(",", matchingphotography);
                    //vendorsPhotography = vendorVenueSignUpService.UpdatePhotography(vendorsPhotography, vendorMaster, long.Parse(id), long.Parse(vid));
                }
                if (matchingdecorators != null)  //if (vendorMaster.ServicType.Split(',').Contains("Decorator"))
                {
                    VendorDecoratorService vendorDecoratorService = new VendorDecoratorService();
                    VendorsDecorator vendorsDecorator = vendorVenueSignUpService.GetParticularVendorDecorator(long.Parse(id), long.Parse(vid));
                    vendorsDecorator.VendorMasterId = vendorMaster.Id;
                    //for (int i = 0; i < matchingphotography.Count; i++)
                    //{
                    var subtype = vendorDecoratorService.GetVendorDecorator(long.Parse(id), long.Parse(vid));
                    vendorsDecorator.DecorationType = string.Join<string>(",", matchingdecorators);
                    if (subtype == null)
                        vendorsDecorator = vendorVenueSignUpService.AddVendorDecorator(vendorsDecorator);
                    else
                        vendorsDecorator = vendorVenueSignUpService.UpdateDecorator(vendorsDecorator, vendorMaster, long.Parse(id), long.Parse(vid));
                    ////}
                    //vendorsDecorator.DecorationType = string.Join<string>(",", matchingdecorators);
                    //vendorsDecorator = vendorVenueSignUpService.UpdateDecorator(vendorsDecorator, vendorMaster, long.Parse(id), long.Parse(vid));
                }
                //return RedirectToAction("Index", "VendorSignUp2",new { id=vendorMaster.Id,vid=vendorVenue.Id});
                return Content("<script language='javascript' type='text/javascript'>alert('General Information Registered Successfully');location.href='" + @Url.Action("Index", "AvailableServices", new { id = vendorMaster.Id }) + "'</script>");
            }
            else
            {
                return RedirectToAction("Index", "HomePage");
            }
        }

        private List<SelectListItem> CountryList()
        {
            List<SelectListItem> cultureList = new List<SelectListItem>();
            CultureInfo[] getCultureInfo = CultureInfo.GetCultures(CultureTypes.SpecificCultures);
            if (getCultureInfo.Count() > 0)
            {
                foreach (CultureInfo cultureInfo in getCultureInfo)
                {
                    RegionInfo getRegionInfo = new RegionInfo(cultureInfo.LCID);
                    var newitem = new SelectListItem { Text = getRegionInfo.EnglishName, Value = getRegionInfo.EnglishName };
                    cultureList.Add(newitem);
                }
            }
            return cultureList;
        }

        public JsonResult checkemail(string emailid)
        {
            VendorMasterService vendorMasterService = new VendorMasterService();
            int query = vendorMasterService.checkemail(emailid);
            if (query != 0)
            {
                return Json("exists", JsonRequestBehavior.AllowGet);
            }
            return Json("valid", JsonRequestBehavior.AllowGet);
        }

    }
}