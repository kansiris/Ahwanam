using MaaAahwanam.Models;
using MaaAahwanam.Service;
using MaaAahwanam.Web.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaaAahwanam.Utility;

namespace MaaAahwanam.Web.Controllers
{
    public class NVendorProfileController : Controller
    {

        OrderService orderService = new OrderService();
        VendorMasterService vendorMasterService = new VendorMasterService();
        UserLoginDetailsService userLoginDetailsService = new UserLoginDetailsService();
        VenorVenueSignUpService venorVenueSignUpService = new VenorVenueSignUpService();
        // GET: NVendorProfile
        public ActionResult Index(string ks)
        {

            try
            {
                if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
                {

                    string strReq = "";
                    encptdecpt encript = new encptdecpt();
                    strReq = encript.Decrypt(ks);
                    //Parse the value... this is done is very raw format.. you can add loops or so to get the values out of the query string...
                    string[] arrMsgs = strReq.Split('&');


                    string[] arrIndMsg;
                    string id = "";
                    arrIndMsg = arrMsgs[0].Split('='); //Get the Name
                    id = arrIndMsg[1].ToString().Trim();

                    var user = (CustomPrincipal)System.Web.HttpContext.Current.User;

                        ViewBag.id = ks;
                        ViewBag.Vendor = vendorMasterService.GetVendor(long.Parse(id));
                        var orders = orderService.userOrderList().Where(m => m.UserLoginId == (int)user.UserId);
                        ViewBag.order = orders.OrderByDescending(m => m.OrderId);
                        ViewBag.profilepic = userLoginDetailsService.GetUser(int.Parse(user.UserId.ToString())).UserImgName;
                    }

                    
                    else
                    {
                        return RedirectToAction("Index", "NUserRegistration");
                    }
                    return View();
                
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Nhomepage");
            }
        }


        public ActionResult updateProfile([Bind(Prefix = "Item1")] Vendormaster vendor)
        {
            try { 
            var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
            string email = vendor.EmailId;

            userLoginDetailsService.Updatevendordetailsnew(vendor, email);
            Int64 id = vendor.Id;
            //TempData["Active"] = "Profile updated";

              return Content("<script language='javascript' type='text/javascript'>alert('Profile updated sucessfully');location.href='" + @Url.Action("Index", "NVendorProfile", new { id = id }) + "'</script>");

                //return RedirectToAction("Index", "NVendorProfile", new { id = id });
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Nhomepage");
            }
        }

        public JsonResult UploadProfilePic(HttpPostedFileBase helpSectionImages, string email)
        {
            string fileName = string.Empty;
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
                UserLoginDetailsService userLoginDetailsService = new UserLoginDetailsService();
                var userdet = userLoginDetailsService.GetUserId(Convert.ToInt32(user.UserId));
                email = userdet.UserName;

                string path = System.IO.Path.GetExtension(helpSectionImages.FileName);
                var filename = email + path;
                fileName = System.IO.Path.Combine(System.Web.HttpContext.Current.Server.MapPath(@"/ProfilePictures/" + filename));
                helpSectionImages.SaveAs(fileName);
                userLoginDetailsService.ChangeDP(int.Parse(user.UserId.ToString()), filename);
            }
            return Json(JsonRequestBehavior.AllowGet);
        }
    }
}