using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaaAahwanam.Utility;

namespace MaaAahwanam.Web.Controllers
{
    public class themeController : Controller
    {
        // GET: theme
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult SendEmail(string fname, string lname, string emailid, string phoneno, string eventdate)
        {
            string ip = HttpContext.Request.UserHostAddress;
            string msg = "First Name: " + fname + ", Last Name : " + lname + ",Email ID : " + emailid + ",Phone Number:" + phoneno + ",Event date:" + eventdate + ",IP:" + ip;
            EmailSendingUtility emailSendingUtility = new EmailSendingUtility();
            string subject = "Ahwanam Party Landing Page";
            emailSendingUtility.Email_maaaahwanam("seema@xsilica.com", msg.Replace(",", "<br/>"), subject);
            emailSendingUtility.Email_maaaahwanam("amit.saxena@ahwanam.com", msg.Replace(",", "<br/>"), subject);
            return Json("success", JsonRequestBehavior.AllowGet);
        }
    }
}