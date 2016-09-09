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

namespace MaaAahwanam.Web.Controllers
{
    public class OrderConfirmationController : Controller
    {
        // GET: OrderConfirmation
        public ActionResult Index()
        {
            int OID = int.Parse(Request.QueryString["oid"]);
            OrderConfirmationService orderConfirmationService = new OrderConfirmationService();
            List<orderconfirmation_Result> list= orderConfirmationService.GetOrderConfirmation(OID);
            ViewBag.Total =list.Sum(i=>i.PerunitPrice);
            ViewBag.Orderconfirmation = list;
            return View();
        }

        public JsonResult EmailOrderConfirmation(string Detdiv)
        {
            StreamReader reader = new StreamReader(Server.MapPath("../Content/EmailTemplates/TempOrderconfirmation.html"));
            string readFile = reader.ReadToEnd();
            string StrContent = "";
            StrContent = readFile;
            StrContent = StrContent.Replace("@@MessageDiv@@", Detdiv);
            string Mailmessage = "<Table>" + Detdiv + "</Table>";
            EmailSendingUtility emailSendingUtility = new EmailSendingUtility();
            emailSendingUtility.Email_maaaahwanam("saikrishna@xsilica.com", StrContent.ToString(), "Test Order Confirmation");
            return Json("Success");
        }
    }
}