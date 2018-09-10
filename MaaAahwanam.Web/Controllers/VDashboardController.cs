using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaaAahwanam.Service;
using MaaAahwanam.Models;
using MaaAahwanam.Web.Custom;
using MaaAahwanam.Utility;

namespace MaaAahwanam.Web.Controllers
{
    public class VDashboardController : Controller
    {
        Vendormaster vendorMaster = new Vendormaster();

        OrderService orderService = new OrderService();
        VendorMasterService vendorMasterService = new VendorMasterService();
        UserLoginDetailsService userLoginDetailsService = new UserLoginDetailsService();
        VenorVenueSignUpService vendorVenueSignUpService = new VenorVenueSignUpService();

        // GET: VDashboard
        public ActionResult Index()
        {
            
                if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
                {


                var user = (CustomPrincipal)System.Web.HttpContext.Current.User;

                string id = user.UserId.ToString(); ;


                string email = userLoginDetailsService.Getusername(long.Parse(id));
                vendorMaster = vendorMasterService.GetVendorByEmail(email);
                ViewBag.Vendor = vendorMasterService.GetVendor(Convert.ToInt64(vendorMaster.Id));
                var orders = orderService.userOrderList().Where(m => m.Id == Convert.ToInt64(vendorMaster.Id));
                ViewBag.currentorders = orders.Where(p => p.Status == "Pending").Count();
                ViewBag.ordershistory = orders.Where(m => m.Status != "Removed").Count();

     
                
                    

                }

                else
                {
                    return RedirectToAction("Index", "NUserRegistration");
                }
                return View();
            

          
        }
    
        public JsonResult UploadProfilePic(HttpPostedFileBase helpSectionImages, string email)
        {
            string fileName = string.Empty;
            string filename = string.Empty;
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
                UserLoginDetailsService userLoginDetailsService = new UserLoginDetailsService();
                var userdet = userLoginDetailsService.GetUserId(Convert.ToInt32(user.UserId));
                email = userdet.UserName;

                string path = System.IO.Path.GetExtension(helpSectionImages.FileName);
               filename = email + path;
                fileName = System.IO.Path.Combine(System.Web.HttpContext.Current.Server.MapPath(@"/ProfilePictures/" + filename));
                if (System.IO.File.Exists(fileName) == true)
                    System.IO.File.Delete(fileName);

                helpSectionImages.SaveAs(fileName);
                userLoginDetailsService.ChangeDP(int.Parse(user.UserId.ToString()), filename);
            }
            return Json(filename, JsonRequestBehavior.AllowGet);
        }
        public ActionResult sidebar()
        {
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {


                


                var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
                string uid = user.UserId.ToString(); 

                string email = userLoginDetailsService.Getusername(long.Parse(uid));
                vendorMaster = vendorMasterService.GetVendorByEmail(email);
                string vid = vendorMaster.Id.ToString();
                ViewBag.Vendor = vendorMasterService.GetVendor(Convert.ToInt64(vendorMaster.Id));
                //ViewBag.id = ks;
                var venues = vendorVenueSignUpService.GetVendorVenue(long.Parse(vid)).ToList();
                var catering = vendorVenueSignUpService.GetVendorCatering(long.Parse(vid)).ToList();
                var photography = vendorVenueSignUpService.GetVendorPhotography(long.Parse(vid));
                var decorators = vendorVenueSignUpService.GetVendorDecorator(long.Parse(vid));
                var others = vendorVenueSignUpService.GetVendorOther(long.Parse(vid));
                ViewBag.venues = venues;
                ViewBag.catering = catering;
                ViewBag.photography = photography;
                ViewBag.decorators = decorators;
                ViewBag.others = others;



            }
            else
            {
                //ViewBag.id = ks;
                ViewBag.Vendor = "";

                ViewBag.profilepic = "";
            }
            return PartialView("sidebar");
            }
        public ActionResult profilepic(string ks)
        {
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {


                


                var user = (CustomPrincipal)System.Web.HttpContext.Current.User;

                string id = user.UserId.ToString();

                string email = userLoginDetailsService.Getusername(long.Parse(id));
                vendorMaster = vendorMasterService.GetVendorByEmail(email);
                ViewBag.Vendor = vendorMasterService.GetVendor(Convert.ToInt64(vendorMaster.Id));

                ViewBag.profilepic = userLoginDetailsService.GetUser(int.Parse(user.UserId.ToString())).UserImgName;
            }
            else
            {
                ViewBag.id = ks;
                ViewBag.Vendor = "";

                ViewBag.profilepic = "";
            }
            return PartialView("profilepic");
        }
        }
    }
