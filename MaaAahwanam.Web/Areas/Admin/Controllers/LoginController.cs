﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaaAahwanam.Models;
using MaaAahwanam.Utility;
using MaaAahwanam.Service;
using System.Configuration;
using System.Web.Security;

namespace MaaAahwanam.Web.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index([Bind(Prefix = "Item1")] UserLogin userLogin,[Bind(Prefix = "Item2")] UserDetail userDetails, string command)
        {
            if (command == "Register")
            {
                UserLoginDetailsService userLoginDetailsService = new UserLoginDetailsService();
                userLogin.UserType = "Admin";
                var response = userLoginDetailsService.AddUserDetails(userLogin, userDetails);
                if (response == "sucess")
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Registered Successfully');location.href='" + @Url.Action("Index", "Login") + "'</script>");
                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Registration Failed');location.href='" + @Url.Action("Index", "Login") + "'</script>");
                }
            }
            if (command == "Authenticate")
            {
                UserLoginDetailsService userLoginDetailsService = new UserLoginDetailsService();
                var response = userLoginDetailsService.AuthenticateUser(userLogin);
                if (response != null)
                {
                    ValidUserUtility.SetAuthCookie(response.UserLoginId.ToString(),response.UserType);
                    Response.Redirect("DashBoard/Dashboard");
                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Wrong Credentials,Check Username and password');location.href='" + @Url.Action("Index", "Login") + "'</script>");
                }
            }
            return View();
        }
        public JsonResult RegularExpressionPattern_Password()
        {
            // Password Reguler Expression Pattern
            return Json(ValidationsUtility.PatternforPassword(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult SignOut()
        {
            //logs.LogTimings("Out");
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Login");
        }

    }
}