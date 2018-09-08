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
        // GET: VDashboard
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
                    arrIndMsg = arrMsgs[0].Split('='); //Get the id
                    id = arrIndMsg[1].ToString().Trim();



                    var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
                    ViewBag.id = ks;
                    ViewBag.Vendor = vendorMasterService.GetVendor(long.Parse(id));
                    var orders = orderService.userOrderList().Where(m => m.Id == int.Parse(id));
                    ViewBag.currentorders = orders.Where(p => p.Status == "Pending").Count();
                    ViewBag.ordershistory = orders.Where(m => m.Status != "Removed").Count();
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
        public ActionResult changeid(string id)
        {
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                string email = userLoginDetailsService.Getusername(long.Parse(id));
                vendorMaster = vendorMasterService.GetVendorByEmail(email);

                string vssid = Convert.ToString(vendorMaster.Id);
                encptdecpt encript = new encptdecpt();

                string encripted = encript.Encrypt(string.Format("Name={0}", vssid));

                vendorMaster = vendorMasterService.GetVendorByEmail(email);
                //return View("AvailableServices", vendorMaster.Id);
                return RedirectToAction("Index", "VDashboard", new { ks = encripted });
            }
            return RedirectToAction("SignOut", "NUserRegistration");
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
                if (System.IO.File.Exists(fileName) == true)
                    System.IO.File.Delete(fileName);

                helpSectionImages.SaveAs(fileName);
                userLoginDetailsService.ChangeDP(int.Parse(user.UserId.ToString()), filename);
            }
            return Json(JsonRequestBehavior.AllowGet);
        }
    }
}