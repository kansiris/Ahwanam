using MaaAahwanam.Models;
using MaaAahwanam.Service;
using MaaAahwanam.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MaaAahwanam.Web.Controllers
{
    public class partnerresgisterController : Controller
    {
        UserLoginDetailsService userLoginDetailsService = new UserLoginDetailsService();

        // GET: partnerresgister
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult vendorreg(string vname, string businessname,string mobile,string email)
        {
            UserLogin userLogin = new UserLogin();
            UserDetail userDetail = new UserDetail();
            userLogin.IPAddress = HttpContext.Request.UserHostAddress;
            userLogin.ActivationCode = Guid.NewGuid().ToString();
            userLogin.Status = "InActive";
            var response = "";
            userLogin.UserType = "Vendor";
            long data = userLoginDetailsService.GetLoginDetailsByEmail(userLogin.UserName);
            if (data == 0)
            { response = userLoginDetailsService.AddUserDetails(userLogin, userDetail); }
            else
            {
                return Json("ks");
                //return Content("<script language='javascript' type='text/javascript'>alert('E-Mail ID Already Registered!!! Try Logging with your Password');location.href='" + @Url.Action("Index", "Home") + "'</script>");

                //TempData["Active"] = "E-Mail ID Already Registered!!! Try Logging with your Password";
                //return RedirectToAction("Index", "Home");
            }
            if (response == "sucess")
            {
             string   activationcode = userLogin.ActivationCode;
               string txtto = userLogin.UserName;
                string username = userDetail.FirstName;
                HomeController home = new HomeController();
                username = home.Capitalise(username);
                string emailid = userLogin.UserName;
                string url = Request.Url.Scheme + "://" + Request.Url.Authority + "/Home/ActivateEmail1?ActivationCode=" + activationcode + "&&Email=" + emailid;
                FileInfo File = new FileInfo(Server.MapPath("/mailtemplate/welcome.html"));
                string readFile = File.OpenText().ReadToEnd();
                readFile = readFile.Replace("[ActivationLink]", url);
                readFile = readFile.Replace("[name]", username);
                string txtmessage = readFile;//readFile + body;
                string subj = "Account Activation";
                EmailSendingUtility emailSendingUtility = new EmailSendingUtility();
                emailSendingUtility.Email_maaaahwanam(txtto, txtmessage, subj);
                //TempData["Active"] = "Check your email to active your account to login";
                //return RedirectToAction("Index", "NUserRegistration");
                return Json("success");
            }
            return Json("success1");
        }
    }
}