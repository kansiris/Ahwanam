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
    public class OrderConfirmationController : Controller
    {
        // GET: OrderConfirmation
        public ActionResult Index()
        {            
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
                int OID = int.Parse(Request.QueryString["oid"]); 
                ViewBag.Oid = OID;
                //OrderConfirmationService orderConfirmationService = new OrderConfirmationService();
                //List<orderconfirmation_Result> list= orderConfirmationService.GetOrderConfirmation(OID);
                DashBoardService dashBoardService = new DashBoardService();
                List<sp_OrderDetails_Result> list = dashBoardService.GetOrderDetailService(OID);
                decimal totalamount = 0;
                foreach (var item in list)
                {
                    if (item.Isdeal == true)
                    {
                        totalamount = item.TotalPrice;
                    }
                    else
                    {
                        totalamount = item.PerunitPrice;
                    }

                }
                ViewBag.Total = totalamount;
                //ViewBag.Total = list[0].TotalPrice;//Select(i=>i.TotalPrice);// list.Sum(i => i.PerunitPrice);
                ViewBag.Orderconfirmation = list;
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Signin");
            }
        }

        public JsonResult EmailOrderConfirmation(string Detdiv,string oid)
        {
            var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
            string testurl = Request.Url.Scheme + "://" + Request.Url.Authority + "/testimonialform?Uid=" + user.UserId + "&Oid=" + oid;
            UserLoginDetailsService userLoginDetailsService = new UserLoginDetailsService();
            string Username = userLoginDetailsService.Getusername(user.UserId);
            StreamReader reader = new StreamReader(Server.MapPath("../Content/EmailTemplates/TempOrderconfirmation.html"));
            string readFile = reader.ReadToEnd();
            string StrContent = "";
            StrContent = readFile + "<h2>Feedback Form</h2>" + testurl;
            //StrContent = readFile + "<h2>Feedback Form</h2>" + "http://localhost:8566/testimonialform?Uid=" + user.UserId + "&Oid=" + oid;
            StrContent = StrContent.Replace("@@MessageDiv@@", Detdiv);
            string Mailmessage = "<Table>" + Detdiv + "</Table>";
            
            EmailSendingUtility emailSendingUtility = new EmailSendingUtility();
            emailSendingUtility.Email_maaaahwanam(Username, StrContent.ToString(), "Test Order Confirmation");
            return Json("Success");
        }
    }
}