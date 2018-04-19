
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaaAahwanam.Utility;

namespace MaaAahwanam.Web.Controllers
{
    public class PreLaunchController : Controller
    {
        // GET: PreLaunch
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult SendEmail(string name, string email, string mobile)
        {
            string msg = "Name: " + name + ", Email-ID : " + email + ", Mobile Number : " + mobile;
            EmailSendingUtility emailSendingUtility = new EmailSendingUtility();
            emailSendingUtility.Email_maaaahwanam("rameshsai@xsilica.com", msg, "Mail From Ahwanam");
            emailSendingUtility.Email_maaaahwanam("info@ahwanam.com", msg, "Mail From Pre-Launch Page");
            return Json(JsonRequestBehavior.AllowGet);
        }
    }
}