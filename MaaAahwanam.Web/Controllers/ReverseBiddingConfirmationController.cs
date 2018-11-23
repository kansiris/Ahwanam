using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaaAahwanam.Models;
using MaaAahwanam.Service;
using MaaAahwanam.Repository;
using MaaAahwanam.Utility;
using System.IO;
using MaaAahwanam.Web.Custom;

namespace MaaAahwanam.Web.Controllers
{
    public class ReverseBiddingConfirmationController : Controller
    {
        //
        // GET: /ReverseBiddingConfirmation/
        public ActionResult Index(ServiceRequest serviceRequest)
        {
            return View(serviceRequest);
        }

        public JsonResult EmailOrderConfirmation(string Detdiv, string oid)
        {
            var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
            UserLoginDetailsService userLoginDetailsService = new UserLoginDetailsService();
            //string Username = userLoginDetailsService.Getusername(user.UserId);
            string Username = oid;
            StreamReader reader = new StreamReader(Server.MapPath("../Content/EmailTemplates/TempOrderconfirmation.html"));
            string readFile = reader.ReadToEnd();
            string StrContent = "";
            StrContent = readFile;
            StrContent = StrContent.Replace("@@MessageDiv@@", Detdiv);
            string Mailmessage = "<Table>" + Detdiv + "</Table>";
            EmailSendingUtility emailSendingUtility = new EmailSendingUtility();
            emailSendingUtility.Email_maaaahwanam(Username, StrContent.ToString(), "Reverse Bidding Confirmation", null);
            return Json("Success");
        }
    }
}