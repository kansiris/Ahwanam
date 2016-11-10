using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MaaAahwanam.Models;
using MaaAahwanam.Utility;
using System.Configuration;
using System.Web.Security;
using Newtonsoft.Json.Linq;
using MaaAahwanam.Service;
using MaaAahwanam.Web.Custom;
using Newtonsoft.Json;

namespace MaaAahwanam.Web.Controllers
{
    public class SigninController : Controller
    {
        static string perfecturl = "";
        static string wrongpwdurl = "";
        static int wrongpwdurlcount = 0;
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Index(string command, [Bind(Prefix = "Item1")] UserLogin userLogin, [Bind(Prefix = "Item2")] UserDetail userDetail,string ReturnUrl)
        {
            if (command == "Register")
            {
                UserLoginDetailsService userLoginDetailsService = new UserLoginDetailsService();
                userLogin.UserType = "User";
                var response = userLoginDetailsService.AddUserDetails(userLogin, userDetail);
                if (response == "sucess")
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Registered Successfully');location.href='" + @Url.Action("Index", "Signin") + "'</script>");
                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Registration Failed');location.href='" + @Url.Action("Index", "Signin") + "'</script>");
                }
            }
            if (command == "AuthenticationUser")
            {
                UserLoginDetailsService userLoginDetailsService = new UserLoginDetailsService();
                userLogin.UserType = "User";
                var userResponse = userLoginDetailsService.AuthenticateUser(userLogin);
                if (userResponse.UserLoginId != 0)
                {
                    wrongpwdurlcount = 0;
                    userResponse.UserType = "User";
                    string userData = JsonConvert.SerializeObject(userResponse);
                    ValidUserUtility.SetAuthCookie(userData, userResponse.UserLoginId.ToString());
                    //string ReturnTo = ReturnUrl;
                    string ReturnTo = perfecturl;
                    if (ReturnTo == null)
                    {
                        Response.Redirect("Index");
                    }
                    else
                    {
                        string[] testid = ReturnTo.Split('/');
                        if (testid.Contains("Signin") == false && testid.Contains("signin") == false)
                            Response.Redirect(ReturnTo);
                        else
                            Response.Redirect(wrongpwdurl);
                    }
                }
                else
                {
                    if (wrongpwdurlcount == 0)
                    { wrongpwdurl = perfecturl; wrongpwdurlcount++; }
                    return Content("<script language='javascript' type='text/javascript'>alert('Wrong Credentials,Check Username and password');location.href='" + @Url.Action("Index", "Signin") + "'</script>");
                    //TempData["Alert"] = "<script language='javascript' type='text/javascript'>alert('Wrong Credentials,Check Username and password')</script>";
                }
            }
            if (command == "AuthenticationVendor")
            {
                UserLoginDetailsService userLoginDetailsService = new UserLoginDetailsService();
                userLogin.UserType = "Vendor";
                var userResponse = userLoginDetailsService.AuthenticateUser(userLogin);
                if (userResponse.UserLoginId != 0)
                {
                    userResponse.UserType = "Vendor";
                    string userData = JsonConvert.SerializeObject(userResponse);
                    ValidUserUtility.SetAuthCookie(userData, userResponse.UserLoginId.ToString());
                    Response.Redirect("VendorDashBoard/Index");
                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Wrong Credentials,Check Username and password');location.href='" + @Url.Action("Index", "Signin") + "'</script>");
                }
            }
            if (command == "" || command == null)
            {
                perfecturl = ReturnUrl;
            }
            return View();
        }

        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Signin");
        }


        [ChildActionOnly]
        public PartialViewResult SigninPartial()
        {
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
                UserLoginDetailsService userLoginDetailsService = new UserLoginDetailsService();
                var response = userLoginDetailsService.GetUser((int)user.UserId);
                if (user.UserType == "Admin")
                {
                    UserDetail userDetail = new UserDetail();
                    return PartialView("SigninPartial", userDetail);
                }
                return PartialView("SigninPartial", response);
            }
            else
            {
                UserDetail userDetail = new UserDetail();
                return PartialView("SigninPartial", userDetail);
            }
        }
        public JsonResult RegularExpressionPattern_Password()
        {
            return Json(ValidationsUtility.PatternforPassword(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult ResetPassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ResetPassword(string sample)
        {
            return View();
        }
    }
}