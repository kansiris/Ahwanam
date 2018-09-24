using MaaAahwanam.Models;
using MaaAahwanam.Service;
using MaaAahwanam.Web.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MaaAahwanam.Web.Controllers
{
    public class ManageUserController : Controller
    {
        Vendormaster vendorMaster = new Vendormaster();
        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        VendorMasterService vendorMasterService = new VendorMasterService();
        UserLoginDetailsService userLoginDetailsService = new UserLoginDetailsService();
        VenorVenueSignUpService vendorVenueSignUpService = new VenorVenueSignUpService();
        VendorDashBoardService mnguserservice = new VendorDashBoardService();
        // GET: ManageUser
        public ActionResult Index(string VendorId)
        {
                var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
                string uid = user.UserId.ToString();
                string vemail = userLoginDetailsService.Getusername(long.Parse(uid));
                vendorMaster = vendorMasterService.GetVendorByEmail(vemail);
                VendorId = vendorMaster.Id.ToString();
                ViewBag.Userlist = mnguserservice.getuser(VendorId);
                return View();
        }
        [HttpPost]
        public ActionResult Index(ManageUser mnguser)
        {
            mnguser.registereddate = DateTime.Now;
            mnguser.updateddate = DateTime.Now;
            mnguser = mnguserservice.AddUser(mnguser);
            return Content("<script language='javascript' type='text/javascript'>alert('Added New User');location.href='/ManageUser'</script>");
        }
        public JsonResult checkemail(string email, string id)
        {
            int query = mnguserservice.checkuseremail(email, id);
            if (query == 0)
                return Json("valid email");
            else
                return Json("already email is added");
        }
     
    }
}