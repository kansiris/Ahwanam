using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaaAahwanam.Utility;

namespace MaaAahwanam.Web.Controllers
{
    public class BirthdayController : Controller
    {
        // GET: Birthday
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult SendEmail(string fname, string lname, string emailid, string phoneno, string eventdate)
        {
            string ip = HttpContext.Request.UserHostAddress;
            string msg = "First Name: " + fname + ", Last Name : " + lname + ",Email ID : " + emailid + ",Phone Number:" + phoneno + ",Event date:" + eventdate + ",IP:" + ip;
            EmailSendingUtility emailSendingUtility = new EmailSendingUtility();
            string subject = "Ahwanam Birthday Landing Page";
            string txtto = "info@ahwanam.com,seema@xsilica.com,amit.saxena@ahwanam.com,dedeepya@gmail.com"; // Mention Target Email ID's here
            emailSendingUtility.Email_maaaahwanam(txtto, msg.Replace(",", "<br/>"), subject);
            return Json("success", JsonRequestBehavior.AllowGet);
        }
    }
}