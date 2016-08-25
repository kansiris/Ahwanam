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
    public class MyProfileController : Controller
    {
        //
        // GET: /MyProfile/
        public ActionResult Index()
        {
            var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
            ViewBag.Type = user.UserType;
            UserLoginDetailsService userLoginDetailsService = new UserLoginDetailsService();
            UserDetail userDetail = userLoginDetailsService.GetUser((int)user.UserId);
            return View(userDetail);
        }
    }
}