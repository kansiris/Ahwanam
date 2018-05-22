using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaaAahwanam.Service;
using MaaAahwanam.Repository;
using MaaAahwanam.Web.Custom;
using MaaAahwanam.Models;
using MaaAahwanam.Utility;
using System.Net;
using System.IO;

namespace MaaAahwanam.Web.Controllers
{
    public class NViewDealController : Controller
    {
        VendorProductsService vendorProductsService = new VendorProductsService();
        UserLoginDetailsService userLoginDetailsService = new UserLoginDetailsService();
        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");


        // GET: NViewDeal
        public ActionResult Index(string id, string type, string eve)
        {
            try
            {
                DateTime date = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);

                //                ViewBag.singledeal = vendorProductsService.getparticulardeal(Int32.Parse(id), type).FirstOrDefault();
                ViewBag.singledeal = vendorProductsService.getpartvendordeal(id, type,date).FirstOrDefault();
                if (eve != "")
                {
                    var data = vendorProductsService.getpartvendordeal(id, type, date).Where(m => m.Category == eve);
                    ViewBag.singledeal1 = data ;
                    ViewBag.events = data.Select(m => m.Category).Distinct();
                    ViewBag.dealLastRecordeve = eve;

                }
                else
                {
                    var data = vendorProductsService.getpartvendordeal(id, type,date);
                    ViewBag.singledeal1 = data;
                    ViewBag.events = data.Select(m => m.Category).Distinct();
                    ViewBag.dealLastRecordeve = "All";
                }

            return View();
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Nhomepage");
            }
        }

        public PartialViewResult Loadmoredeals(string lastrecord, string eve)
        {
            int id = (lastrecord == null) ? 2 : int.Parse(lastrecord) + 2;
            if (eve == null || eve == "") { eve = "All"; }
            if (eve == "KittyCocktailsParties") { eve = "Kitty Cocktails Parties"; }
            if (eve == "CorporatesCocktailparties") { eve = "Corporates Cocktail parties"; }
            if (eve == "CorporateBuffetLunch/Dinner") { eve = "Corporate Buffet Lunch / Dinner"; }
            if (eve == "AnnualDayCelebrations") { eve = "Annual Day Celebrations"; }
            if (eve == "HoneymoonpackagesRoomsCPPlan") { eve = "Honeymoon packages Rooms CP Plan"; }

            DateTime date = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);

            //ViewBag.deal = vendorProductsService.getalldeal().OrderBy(m => m.DealID).Take(id);
            //var deals = vendorProductsService.getalldeal().OrderBy(m => m.DealID).Take(id);

            //ViewBag.dealLastRecord = id;
            // ViewBag.dealcount = vendorProductsService.getalldeal().Count();

            var deals = vendorProductsService.getalleventdeal(eve,date).OrderBy(m => m.DealID).Take(id);
            ViewBag.deal = deals;
            ViewBag.dealLastRecord = id;
            ViewBag.dealcount = vendorProductsService.getalleventdeal(eve,date).Count();
            return PartialView("Loadmoredeals");
        }

        public ActionResult booknow(string type, string etype1, string date, string totalprice, string id,string price, string guest, string timeslot,string vid, string did)
     {

            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
                // string updateddate = DateTime.UtcNow.ToShortDateString();

                DateTime updateddate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
                int userid = Convert.ToInt32(user.UserId);
                //Saving Record in order Table
                OrderService orderService = new OrderService();
                Order order = new Order();
                order.TotalPrice = Convert.ToDecimal(totalprice);
                order.OrderDate = Convert.ToDateTime(updateddate); //Convert.ToDateTime(bookeddate);
                order.UpdatedBy = (Int64)user.UserId;
                order.OrderedBy = (Int64)user.UserId;
                order.UpdatedDate = Convert.ToDateTime(updateddate);
                order.Status = "Pending";
                order = orderService.SaveOrder(order);

                //Payment Section
                Payment_orderServices payment_orderServices = new Payment_orderServices();
                Payment_Orders payment_Orders = new Payment_Orders();
                payment_Orders.cardnumber = "4222222222222";
                payment_Orders.CVV = "214";
                payment_Orders.paidamount = decimal.Parse(totalprice);
                //payment_Orders.PaymentID = orderRequest.PaymentId;
                payment_Orders.Paiddate = Convert.ToDateTime(updateddate);
                payment_Orders.OrderID = order.OrderId;
                payment_Orders = payment_orderServices.SavePayment_Orders(payment_Orders);

                //Saving Order Details
                OrderdetailsServices orderdetailsServices = new OrderdetailsServices();
                OrderDetail orderDetail = new OrderDetail();
                orderDetail.OrderId = order.OrderId;
                orderDetail.OrderBy = user.UserId;
                orderDetail.PaymentId = payment_Orders.PaymentID;
                orderDetail.ServiceType = type;
                orderDetail.ServicePrice = decimal.Parse(price);
                orderDetail.attribute = timeslot;
                orderDetail.TotalPrice = decimal.Parse(totalprice);
                orderDetail.PerunitPrice = decimal.Parse(price);
                orderDetail.Quantity = int.Parse(guest);
                orderDetail.OrderId = order.OrderId;
                orderDetail.VendorId = long.Parse(id);
                orderDetail.Status = "Pending";
                orderDetail.UpdatedDate = Convert.ToDateTime(updateddate);
                orderDetail.UpdatedBy = user.UserId;
                orderDetail.subid = long.Parse(vid);
                orderDetail.BookedDate = Convert.ToDateTime(date);
                orderDetail.EventType = etype1;
                orderDetail.DealId = long.Parse(did);
                orderdetailsServices.SaveOrderDetail(orderDetail);

               var userlogdetails = userLoginDetailsService.GetUserId(userid);

                string   txtto = userlogdetails.UserName;
                var userdetails = userLoginDetailsService.GetUser(userid);
                string name = userdetails.FirstName;
                name = Capitalise(name);
                string OrderId = Convert.ToString(order.OrderId);
                string url = Request.Url.Scheme + "://" + Request.Url.Authority;
                FileInfo File = new FileInfo(Server.MapPath("/mailtemplate/order.html"));
                string readFile = File.OpenText().ReadToEnd();
                readFile = readFile.Replace("[ActivationLink]", url);
                readFile = readFile.Replace("[name]", name);
                readFile = readFile.Replace("[orderid]", OrderId);
                string txtmessage = readFile;//readFile + body;
                string subj = "Thanks for your order";
                EmailSendingUtility emailSendingUtility = new EmailSendingUtility();
                emailSendingUtility.Email_maaaahwanam(txtto, txtmessage, subj);

                var vendordetails = userLoginDetailsService.getvendor(Convert.ToInt32(id));

                string txtto1 = vendordetails.EmailId ;
                string vname = vendordetails.BusinessName;
                vname = Capitalise(vname);

                string url1 = Request.Url.Scheme + "://" + Request.Url.Authority;
                FileInfo file1 = new FileInfo(Server.MapPath("/mailtemplate/vorder.html"));
                string readfile1 = file1.OpenText().ReadToEnd();
                readfile1 = readfile1.Replace("[ActivationLink]", url1);
                readfile1 = readfile1.Replace("[name]", name);
                readfile1 = readfile1.Replace("[vname]", vname);
                readfile1 = readfile1.Replace("[orderid]", OrderId);
                string txtmessage1 = readfile1;
                string subj1 = "order has been placed";
                emailSendingUtility.Email_maaaahwanam(txtto1, txtmessage1, subj1);

                return Json("Success", JsonRequestBehavior.AllowGet);
            }
            return Json(JsonRequestBehavior.AllowGet);
        }
        public string Capitalise(string str)
        {
            if (String.IsNullOrEmpty(str))
                return String.Empty;
            return Char.ToUpper(str[0]) + str.Substring(1).ToLower();
        }
        public ActionResult addcnow(string type, string etype1, string date, string totalprice, string id, string price, string guest, string timeslot, string vid, string did)
        {

            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
                var vendor = vendorProductsService.getparticulardeal(Int32.Parse(id), type).FirstOrDefault();
                // string updateddate = DateTime.UtcNow.ToShortDateString();
                DateTime updateddate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
                CartItem cartItem = new CartItem();
                cartItem.VendorId = Int32.Parse(id);
                cartItem.ServiceType = etype1;
                cartItem.TotalPrice = decimal.Parse(totalprice);
                cartItem.Orderedby = user.UserId;
                cartItem.UpdatedDate = Convert.ToDateTime(updateddate);
                cartItem.Perunitprice = decimal.Parse(price);
                cartItem.Quantity = Convert.ToInt16(guest);
                cartItem.subid = Convert.ToInt64(vid);
                //  cartItem.attribute = orderRequest.attribute;
                cartItem.DealId = Convert.ToInt64(did);
                CartService cartService = new CartService();
                cartItem = cartService.AddCartItem(cartItem);

                return Json("Success", JsonRequestBehavior.AllowGet);
            }
            return Json(JsonRequestBehavior.AllowGet);
        }

        public ActionResult sort(string id, string type, string eve)
        {
            DateTime date = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);

            var data = vendorProductsService.getpartvendordeal(id, type,date).Where(m => m.Category == eve);
            var message = String.Join("~", data.Select(m => new  {   m.DealPrice,  m.FoodType, m.DealID }));
            return Json(message,JsonRequestBehavior.AllowGet);
        }

    }
}