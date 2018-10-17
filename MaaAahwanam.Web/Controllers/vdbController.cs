using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaaAahwanam.Service;
using MaaAahwanam.Models;
using MaaAahwanam.Web.Custom;
using MaaAahwanam.Utility;
using System.IO;
using MaaAahwanam.Repository;

namespace MaaAahwanam.Web.Controllers
{
    public class vdbController : Controller
    {
        public CustomPrincipal user = null;
        Vendormaster vendorMaster = new Vendormaster();
        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        UserLoginDetailsService userLoginDetailsService = new UserLoginDetailsService();
        VendorMasterService vendorMasterService = new VendorMasterService();
        VenorVenueSignUpService vendorVenueSignUpService = new VenorVenueSignUpService();
        VendorImageService vendorImageService = new VendorImageService();
        const string imagepath = @"/vendorimages/";

        // GET: vdb
        public ActionResult Index(string c, string vid)
        {
            long id = GetVendorID(); // Here Vendor Master ID will be Retrieved
            if (id != 0)
            {
                #region c == "first" 
                if (c == "first")
                {
                    // Retrieving Particular Vendor Details
                    var vendordata = vendorMasterService.GetVendor(Convert.ToInt64(id)); 
                    ViewBag.Vendor = vendordata;
                    if (vid != null)
                    {
                        VendorVenueService vendorVenueService = new VendorVenueService();
                        // Retrieving Particular Service Info
                        var servicedata = vendorVenueService.GetVendorVenue(id, long.Parse(vid));

                        // Retrieving Hall name or VenueType
                        if (servicedata.name != null && servicedata.name != "")
                            ViewBag.name = servicedata.name;
                        else
                            ViewBag.name = servicedata.VenueType;

                        // Retrieving All Images
                        var allimages = vendorImageService.GetImages(id, long.Parse(vid));
                        if (allimages.Count() > 0)
                        {
                            ViewBag.bannerimage = "/vendorimages/" + allimages.FirstOrDefault().ImageName;
                            ViewBag.allimages = allimages.ToList();
                            ViewBag.imagescount = (allimages.Count < 4) ? 4 - allimages.Count : 0;
                            ViewBag.sliderimages = allimages.Where(m => m.ImageType == "Slider").Take(4).ToList();
                            ViewBag.slidercount = (ViewBag.sliderimages.Count < 4) ? 4 - ViewBag.sliderimages.Count : 0;
                        }
                        else
                        {
                            ViewBag.bannerimage = "~/newdesignstyles/images/banner3.jpg";
                            ViewBag.imagescount = ViewBag.sliderimages = 0;
                            ViewBag.slidercount = 4;
                        }

                        ViewBag.id = id;   // Assigning Vendor Master ID to viewbag
                        ViewBag.vid = vid; // Assigning Vendor Service ID to viewbag
                    }
                }
                #endregion
                ViewBag.enable = c;
                return View();
            }
            return Content("<script>alert('Session Timeout!!! Please Login'); location.href='/home'</script>");
        }

        public ActionResult profilepic()
        {
            long id = GetVendorID();
            if (id != 0)
            {
                ViewBag.Vendor = vendorMasterService.GetVendor(Convert.ToInt64(id));
                ViewBag.profilepic = userLoginDetailsService.GetUser(int.Parse(user.UserId.ToString())).UserImgName;
                return PartialView("profilepic");
            }
            return Content("<script>alert('Session Timeout!!! Please Login'); location.href='/home'</script>");
        }

        public ActionResult sidebar()
        {
            long id = GetVendorID();
            if (id != 0)
            {
                ViewBag.venues = vendorVenueSignUpService.GetVendorVenue(id).ToList();
                ViewBag.catering = vendorVenueSignUpService.GetVendorCatering(id).ToList();
                ViewBag.photography = vendorVenueSignUpService.GetVendorPhotography(id);
                ViewBag.decorators = vendorVenueSignUpService.GetVendorDecorator(id);
                ViewBag.others = vendorVenueSignUpService.GetVendorOther(id);
                return PartialView("sidebar");
            }
            return Content("<script>alert('Session Timeout!!! Please Login'); location.href='/home'</script>");
        }

        public long GetVendorID()
        {
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                user = (CustomPrincipal)System.Web.HttpContext.Current.User;
                string id = user.UserId.ToString();
                string email = userLoginDetailsService.Getusername(long.Parse(id));
                vendorMaster = vendorMasterService.GetVendorByEmail(email);
                return vendorMaster.Id;
            }
            return 0;
        }
    }
}