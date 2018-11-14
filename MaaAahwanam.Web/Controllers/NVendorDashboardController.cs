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
    public class NVendorDashboardController : Controller
    {
        OrderService orderService = new OrderService();
        VendorMasterService vendorMasterService = new VendorMasterService();
        UserLoginDetailsService userLoginDetailsService = new UserLoginDetailsService();

        // GET: NVendorDashboard
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
                        var orders = orderService.userOrderList().Where(m => m.vid == int.Parse(id));
                        ViewBag.currentorders = orders.Where(p => p.orderstatus == "Pending").Count();
                        ViewBag.ordershistory = orders.Where(m => m.orderstatus != "Removed").Count();
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

        public ActionResult VendorAuth()
        {
            try
            {
                if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
                    var vendorrecord = userLoginDetailsService.GetUser(int.Parse(user.UserId.ToString()));
                    ViewBag.profilepic = vendorrecord.UserImgName;
                    var emailid = vendorrecord.AlternativeEmailID;
                    // ViewBag.id = vendorMasterService.GetVendorByEmail(emailid).Id;

                    string vssid = Convert.ToString(vendorMasterService.GetVendorByEmail(emailid).Id) ;
                    encptdecpt encript = new encptdecpt();

                    string encripted = encript.Encrypt(string.Format("Name={0}", vssid));
                    ViewBag.id = encripted;
                }
                return PartialView("VendorAuth");
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Nhomepage");
            }
        }

        public ActionResult checkuser(string ks)
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
                string email = userLoginDetailsService.Getusername(long.Parse(id));
                Vendormaster vendorMaster = vendorMasterService.GetVendorByEmail(email);
                ViewBag.vendorid = vendorMaster.Id;
                
                //return View("AvailableServices", vendorMaster.Id);
                //return RedirectToAction("Index", "NVendorDashboard", new { id = vendorMaster.Id });
            }
            return RedirectToAction("SignOut", "NUserRegistration");
        }
    }
}