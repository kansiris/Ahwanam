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
        // GET: VendorSignUp1
        public ActionResult Index()
        {
            ViewBag.country = new SelectList(CountryList(), "Value", "Text");
            return View();
        }
        [HttpPost]
        public ActionResult Index([Bind(Prefix = "Item1")] Vendormaster vendorMaster, [Bind(Prefix = "Item2")] UserLogin userLogin, [Bind(Prefix = "Item3")]UserDetail userDetail, [Bind(Prefix = "Item4")]VendorVenue vendorVenue)
        {
            string[] venueservices = { "Convention Hall", "Function Hall", "Banquet Hall", "Meeting Room", "Open Lawn", "Roof Top", "Hotel", "Resort" };
            string[] cateringservices = { "Indian", "Chinese", "Mexican", "South Indian", "Continental", "Multi Cuisine", "Chaat", "Fast Food", "Others" };
            string[] photographyservices = { "Wedding", "Candid", "Portfolio", "Fashion", "Toddler", "Videography", "Conventional", "Cinematography", "Others" };
            List<string> matchingvenues = venueservices.Intersect(vendorVenue.VenueType.Split(',')).ToList();
            List<string> matchingcatering = cateringservices.Intersect(vendorVenue.VenueType.Split(',')).ToList();
            List<string> matchingphotography = photographyservices.Intersect(vendorVenue.VenueType.Split(',')).ToList();
            userLogin = vendorVenueSignUpService.AddUserLogin(userLogin);
            userDetail.UserLoginId = userLogin.UserLoginId;
            userDetail = vendorVenueSignUpService.AddUserDetail(userDetail, vendorMaster);
            vendorMaster = vendorVenueSignUpService.AddvendorMaster(vendorMaster);
            if (vendorMaster.ServicType.Split(',').Contains("Venue"))
            {
                vendorVenue.VenueType = string.Join<string>(",", matchingvenues);
                vendorVenue.VendorMasterId = vendorMaster.Id;
                vendorVenue = vendorVenueSignUpService.AddVendorVenue(vendorVenue);
            }
            if (vendorMaster.ServicType.Split(',').Contains("Catering"))
            {
                VendorsCatering vendorsCatering = new VendorsCatering();
                vendorsCatering.VendorMasterId = vendorMaster.Id;
                vendorsCatering.CuisineType = string.Join<string>(",", matchingcatering);
                vendorsCatering = vendorVenueSignUpService.AddVendorCatering(vendorsCatering);
            }
            if (vendorMaster.ServicType.Split(',').Contains("Photography"))
            {
                VendorsPhotography vendorsPhotography = new VendorsPhotography();
                vendorsPhotography.VendorMasterId = vendorMaster.Id;
                vendorsPhotography.PhotographyType = string.Join<string>(",", matchingphotography);
                vendorsPhotography = vendorVenueSignUpService.AddVendorPhotography(vendorsPhotography);
            }
            //return RedirectToAction("Index", "VendorSignUp2",new { id=vendorMaster.Id,vid=vendorVenue.Id});
            return Content("<script language='javascript' type='text/javascript'>alert('General Information Registered Successfully');location.href='" + @Url.Action("Index", "VendorSignUp4", new { id = vendorMaster.Id}) + "'</script>"); 
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