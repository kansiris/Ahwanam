using MaaAahwanam.Models;
using MaaAahwanam.Service;
using MaaAahwanam.Utility;
using MaaAahwanam.Web.Custom;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MaaAahwanam.Web.Controllers
{
    public class ManageStaffController : Controller
    {
        UserLogin userLogin = new UserLogin();
        VenorVenueSignUpService venorVenueSignUpService = new VenorVenueSignUpService();
        UserDetail userDetail = new UserDetail();
        newmanageuser newmanageuse = new newmanageuser();
        Vendormaster vendorMaster = new Vendormaster();
        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        VendorDashBoardService mnguserservice = new VendorDashBoardService();
        UserLoginDetailsService userLoginDetailsService = new UserLoginDetailsService();
        // GET: ManageStaff
        public ActionResult Index(string sid)
        {
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
                string uid = user.UserId.ToString();
                string vemail = newmanageuse.Getusername(long.Parse(uid));
                vendorMaster = newmanageuse.GetVendorByEmail(vemail);
                string VendorId = vendorMaster.Id.ToString();
                ViewBag.masterid = VendorId;
                ViewBag.Userlist = mnguserservice.getuser(VendorId);
                var getstafflist = mnguserservice.getstaff(VendorId);
                ViewBag.getstafflist = getstafflist;

                if (sid != null && sid != "")
                {

                    var data = mnguserservice.getstaffbyid(int.Parse(sid));
                    userLogin.UserName = data.FirstOrDefault().emailid;
                    var userResponse = venorVenueSignUpService.GetUserLogdetails(userLogin);
                    ViewBag.password = userResponse.Password;
                    ViewBag.details = data;

                }
                else
                {

                    ViewBag.password = null;
                    ViewBag.details = null;
                }
                return View();

            }
            else { return RedirectToAction("index", "home"); }
        }

        [HttpPost]
        public JsonResult addupdatestaff(StaffAccess Staffsccess, string id, string command,string kscadd,string password)
        {
            string msg = string.Empty;
            Staffsccess.UpdatedDate = DateTime.Now;
            Staffsccess.RegisteredDate = DateTime.Now;
            if (command == "Save")
            {
               
                userLogin.IPAddress = HttpContext.Request.UserHostAddress;
                userLogin.ActivationCode = Guid.NewGuid().ToString();
                userDetail.FirstName = Staffsccess.fname;
                userDetail.UserPhone = Staffsccess.phoneno;
                userLogin.Password = password;
                userLogin.UserName = Staffsccess.emailid;
                if (kscadd.Contains("Orders_view")) Staffsccess.order = 1 ;
                if (kscadd.Contains("Orders_add")) Staffsccess.order = 2;
                if (kscadd.Contains("book_view")) Staffsccess.book = 1;
                if (kscadd.Contains("book_add")) Staffsccess.book = 2;
                if (kscadd.Contains("quote_view")) Staffsccess.quote = 1;
                if (kscadd.Contains("quote_add")) Staffsccess.quote = 2;
                if (kscadd.Contains("service_view")) Staffsccess.service = 1;
                if (kscadd.Contains("service_add")) Staffsccess.service = 2;
                if (kscadd.Contains("RevenueModel_view")) Staffsccess.revenuemodel = 1;
                if (kscadd.Contains("RevenueModel_add")) Staffsccess.revenuemodel = 2;
                if (kscadd.Contains("invoice_view")) Staffsccess.invoice = 1;
                if (kscadd.Contains("invoice_add")) Staffsccess.invoice = 2;
                if (kscadd.Contains("payment_view")) Staffsccess.payment = 1;
                if (kscadd.Contains("payment_add")) Staffsccess.payment = 2;
                if (kscadd.Contains("customer_view")) Staffsccess.customer = 1;
                if (kscadd.Contains("customer_add")) Staffsccess.customer = 2;
                if (kscadd.Contains("supplier_view")) Staffsccess.supplier = 1;
                if (kscadd.Contains("supplier_add")) Staffsccess.supplier = 2;
                if (kscadd.Contains("addstaff_view")) Staffsccess.addstaff = 1;
                if (kscadd.Contains("addstaff_view")) Staffsccess.addstaff = 2;
                userLogin.Status = "Active";
                var response = "";
                userLogin.UserType = "VendorStaff";
                DateTime updateddate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);//DateTime.UtcNow.ToShortDateString();
                userLogin.RegDate = updateddate;
                userLogin.UpdatedDate = updateddate;
                long data = userLoginDetailsService.GetLoginDetailsByEmail(Staffsccess.emailid);
                if (data == 0)
                { 
                    userLogin = newmanageuse.addloginstaff(userLogin);
                    Staffsccess = newmanageuse.Savestaff(Staffsccess);
                    msg = "Added New staff";
                }
                else
                {
                    msg = "Unique";
                }
               
                   
            }
            else if (command == "Update")
            {
                if (kscadd.Contains("Orders_view")) Staffsccess.order = 1;
                if (kscadd.Contains("Orders_add")) Staffsccess.order = 2;
                if (kscadd.Contains("book_view")) Staffsccess.book = 1;
                if (kscadd.Contains("book_add")) Staffsccess.book = 2;
                if (kscadd.Contains("quote_view")) Staffsccess.quote = 1;
                if (kscadd.Contains("quote_add")) Staffsccess.quote = 2;
                if (kscadd.Contains("service_view")) Staffsccess.service = 1;
                if (kscadd.Contains("service_add")) Staffsccess.service = 2;
                if (kscadd.Contains("RevenueModel_view")) Staffsccess.revenuemodel = 1;
                if (kscadd.Contains("RevenueModel_add")) Staffsccess.revenuemodel = 2;
                if (kscadd.Contains("invoice_view")) Staffsccess.invoice = 1;
                if (kscadd.Contains("invoice_add")) Staffsccess.invoice = 2;
                if (kscadd.Contains("payment_view")) Staffsccess.payment = 1;
                if (kscadd.Contains("payment_add")) Staffsccess.payment = 2;
                if (kscadd.Contains("customer_view")) Staffsccess.customer = 1;
                if (kscadd.Contains("customer_add")) Staffsccess.customer = 2;
                if (kscadd.Contains("supplier_view")) Staffsccess.supplier = 1;
                if (kscadd.Contains("supplier_add")) Staffsccess.supplier = 2;
                if (kscadd.Contains("addstaff_view")) Staffsccess.addstaff = 1;
                if (kscadd.Contains("addstaff_view")) Staffsccess.addstaff = 2;
                Staffsccess = newmanageuse.updatestaff(Staffsccess, int.Parse(id));
                msg = "Updated staff";
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetstaffDetails(string id)
        {
           
            ViewBag.details1 = "Data1";

            return RedirectToAction("Index", "ManageStaff");
        }
    }
}