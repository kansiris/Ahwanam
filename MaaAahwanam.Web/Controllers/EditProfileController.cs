using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaaAahwanam.Models;
using MaaAahwanam.Utility;
using System.Configuration;
using System.Web.Security;
using MaaAahwanam.Service;
using MaaAahwanam.Web.Custom;

namespace MaaAahwanam.Web.Controllers
{
    [Authorize]
    public class EditProfileController : Controller
    {
        UserLoginDetailsService userLoginDetailsService = new UserLoginDetailsService();
        //
        // GET: /EditProfile/
        public ActionResult Index()
        {
            var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
            UserDetail userDetail = userLoginDetailsService.GetUser((int)user.UserId);
            ViewBag.Type = user.UserType;      
            return View(userDetail);
        }
        [HttpPost]
        public ActionResult Index(UserDetail userDetail)
        {
            var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
            userLoginDetailsService.UpdateUserdetails(userDetail, (int)user.UserId);
            return Content("<script language='javascript' type='text/javascript'>alert('Updated Successfully');location.href='" + @Url.Action("Index", "EditProfile") + "'</script>");

        }
    }
}