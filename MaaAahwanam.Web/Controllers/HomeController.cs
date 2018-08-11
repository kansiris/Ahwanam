using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaaAahwanam.Service;
using MaaAahwanam.Utility;

namespace MaaAahwanam.Web.Controllers
{
    public class HomeController : Controller
    {
        ResultsPageService resultsPageService = new ResultsPageService();
        // GET: Home
        public ActionResult Index()
        {
            var data = resultsPageService.GetAllVendors("Venue").ToList();
            Random r = new Random();
            int rInt = r.Next(0, data.Count);
            ViewBag.venues = data.Skip(rInt).Take(3).ToList();
            return View();
        }

        [HttpPost]
        public ActionResult Index(string command)
        {
            return RedirectToAction("Index", "results");
        }




        public ActionResult SendEmail(string name, string number, string city, string eventtype, string datepicker2)
        {
            string ip = HttpContext.Request.UserHostAddress;
            string msg = "Name: " + name + ", Mobile Number : " + number + ",City : " + city + ",Event Type:" + eventtype + ",Event date:" + datepicker2 + ",IP:" + ip;
            EmailSendingUtility emailSendingUtility = new EmailSendingUtility();
            emailSendingUtility.Email_maaaahwanam("rameshsai@xsilica.com", msg.Replace(",", "<br/>"), "Mail From Ahwanam");
            emailSendingUtility.Email_maaaahwanam("seema@xsilica.com", msg.Replace(",", "<br/>"), "Mail From Ahwanam");
            emailSendingUtility.Email_maaaahwanam("amit.saxena@ahwanam.com", msg.Replace(",", "<br/>"), "Mail From Ahwanam");
            return Content("<script language='javascript' type='text/javascript'>alert('Details Sent Successfully!!!Click OK and Explore Ahwanam.com');location.href='" + @Url.Action("Index", "Home") + "'</script>");
        }
    }
}