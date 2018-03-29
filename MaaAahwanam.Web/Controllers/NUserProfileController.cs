using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaaAahwanam.Web.Custom;
using MaaAahwanam.Service;
using System.Net;
using MaaAahwanam.Models;
using MaaAahwanam.Utility;
using System.Text.RegularExpressions;

namespace MaaAahwanam.Web.Controllers
{
    public class NUserProfileController : Controller
    {
        UserLoginDetailsService userLoginDetailsService = new UserLoginDetailsService();

        // GET: NUserProfile
        public ActionResult Index()
        {
            var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
            var userdata = userLoginDetailsService.GetUser((int)user.UserId);
            if (userdata.FirstName != "" && userdata.FirstName != null)
                ViewBag.username = userdata.FirstName;
            else if (userdata.FirstName != "" && userdata.FirstName != null && userdata.LastName != "" && userdata.LastName != null)
                ViewBag.username = "" + userdata.FirstName + " " + userdata.LastName + "";
            else
                ViewBag.username = userdata.AlternativeEmailID;

            var userdata1 = userLoginDetailsService.GetUserId((int)user.UserId);
            ViewBag.username = userdata1.UserName;


            return View();
        }

        public ActionResult ChangePassword(UserLogin userLogin)
        {
            var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
            UserLoginDetailsService userLoginDetailsService = new UserLoginDetailsService();
            userLoginDetailsService.changepassword(userLogin, (int)user.UserId);
              return Content("<script language='javascript' type='text/javascript'>alert('Password Updated Successfully');location.href='" + @Url.Action("Index", "NUserProfile") + "'</script>");
           
        }
    }
}