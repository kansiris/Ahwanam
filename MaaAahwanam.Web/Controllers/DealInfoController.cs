using MaaAahwanam.Repository;
using MaaAahwanam.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaaAahwanam.Models;
using MaaAahwanam.Utility;
using MaaAahwanam.Web.Custom;

namespace MaaAahwanam.Web.Controllers
{
    public class DealInfoController : Controller
    {
        ReviewService reviewService = new ReviewService();
        //
        // GET: /CardInfo/
        public ActionResult Index()
        {
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
                ViewBag.Userloginstatus = user.UserId;
            }
            else
            {
                ViewBag.Userloginstatus = 0;
            }
            ProductInfoService productInfoService = new ProductInfoService();
            VendorVenueService vendorVenueService = new VendorVenueService();
            Review review = new Review();
            string Servicetype = Request.QueryString["par"];
            int vid = Convert.ToInt32(Request.QueryString["VID"]);
            int Svid = Convert.ToInt32(Request.QueryString["subvid"]);
            int did = Convert.ToInt32(Request.QueryString["did"]);
            ViewBag.Subvid = Svid;
            if (Servicetype == "Travel&Accomadation")
            {
                Servicetype = "Travel";
            }
           
            SP_dealsinfo_Result Dealinfo = productInfoService.getDealsInfo_Result(vid, Servicetype, Svid,did);
            ViewBag.discountvalue = 10.00;
            if (Dealinfo.ActualServiceprice != 0 && Dealinfo.DealServiceprice != 0)
            { 
                string discountvalue=(((Dealinfo.ActualServiceprice - Dealinfo.DealServiceprice) / Dealinfo.ActualServiceprice) * 100).Value.ToString("0.00");
                ViewBag.discountvalue = discountvalue;
            }
            if (Dealinfo.image != null)
            {
                string[] imagenameslist = Dealinfo.image.Replace(" ", "").Split(',');
                ViewBag.Imagelist = imagenameslist;
            }
            ViewBag.servicetype = Servicetype;
            ViewBag.Reviewlist = reviewService.GetReview(vid);
            if (Dealinfo.ServicType == "Venue")
            {
                var list = vendorVenueService.GetVendorVenue(vid, Svid);
                ViewBag.venuetype = list.VenueType;
                ViewBag.servicecost = list.ServiceCost;
            }
            var tupleModel = new Tuple<SP_dealsinfo_Result, Review>(Dealinfo, review);
            return View(tupleModel);
        }
        public ActionResult WriteaRiview([Bind(Prefix = "Item2")] Review review,string did)
        {
            var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
            review.UpdatedBy = (int)user.UserId;
            //int did = Convert.ToInt32(Request.QueryString["did"]);
            review.Status = "Active";
            review.UpdatedDate = DateTime.Now;
            reviewService.InsertReview(review);
            return RedirectToAction("Index", new { par = review.Service, VID = review.ServiceId, subvid = review.Sid });

            //return RedirectToAction("Index", "Signin");
        }
        public JsonResult Addtocart(OrderRequest orderRequest)
        {
            var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
            CartItem cartItem = new CartItem();
            cartItem.VendorId = orderRequest.VendorId;
            cartItem.ServiceType = orderRequest.ServiceType;
            cartItem.Perunitprice = orderRequest.Perunitprice;
            cartItem.TotalPrice = orderRequest.TotalPrice;
            cartItem.Orderedby = user.UserId;
            cartItem.Quantity = orderRequest.Quantity;
            cartItem.UpdatedDate = DateTime.Now;
            cartItem.subid = orderRequest.subid;
            cartItem.attribute = orderRequest.attribute;
            cartItem.Isdeal = true;
            CartService cartService = new CartService();
            cartItem = cartService.AddCartItem(cartItem);
            EventInformation eventInformation = new EventInformation();
            eventInformation.EventName = orderRequest.EventName;
            eventInformation.Email = orderRequest.Email;
            eventInformation.Address = orderRequest.Address;
            eventInformation.Location = orderRequest.Location;
            eventInformation.Phone = orderRequest.Phone;
            eventInformation.PostalCode = orderRequest.PostalCode;
            eventInformation.State = orderRequest.State;
            eventInformation.City = orderRequest.City;
            eventInformation.vendorid = orderRequest.VendorId;
            eventInformation.subid = orderRequest.subid;
            eventInformation.CartId = cartItem.CartId;

            EventsService eventsService = new EventsService();
            eventInformation = eventsService.SaveEventinformation(eventInformation);

            EventDate eventDate = new EventDate();
            foreach (var item in orderRequest.EventDates)
            {
                eventDate.StartDate = item.StartDate;
                eventDate.StartTime = item.StartTime;
                eventDate.EndDate = item.EndDate;
                eventDate.EndTime = item.EndTime;
                eventDate.vendorid = orderRequest.VendorId;
                eventDate.EventId = eventInformation.EventId;
            }


            EventDatesServices eventDatesServices = new EventDatesServices();
            string message3 = eventDatesServices.SaveEventDates(eventDate);
            return Json(message3);
        }
        [Authorize]
        public JsonResult Buynow(OrderRequest orderRequest)
        {
            var user = (CustomPrincipal)System.Web.HttpContext.Current.User;

            OrderService orderService = new OrderService();
            Order order = new Order();
            order.TotalPrice = Convert.ToDecimal(orderRequest.TotalPrice);
            order.OrderDate = DateTime.Now;
            order.UpdatedBy = (Int64)user.UserId;
            order.OrderedBy = (Int64)user.UserId;
            order.UpdatedDate = DateTime.Now;
            order.Status = "Active";
            order = orderService.SaveOrder(order);

            Payment_orderServices payment_orderServices = new Payment_orderServices();
            Payment_Orders payment_Orders = new Payment_Orders();
            payment_Orders.cardnumber = orderRequest.cardnumber;
            payment_Orders.CVV = orderRequest.CVV;
            payment_Orders.paidamount = orderRequest.TotalPrice;
            payment_Orders.PaymentID = orderRequest.PaymentId;
            payment_Orders.Paiddate = orderRequest.Paiddate;
            payment_Orders.OrderID = order.OrderId;
            payment_Orders = payment_orderServices.SavePayment_Orders(payment_Orders);

            OrderdetailsServices orderdetailsServices = new OrderdetailsServices();
            OrderDetail orderDetail = new OrderDetail();
            orderDetail.OrderId = order.OrderId;
            orderDetail.OrderBy = user.UserId;
            orderDetail.PaymentId = payment_Orders.PaymentID;
            orderDetail.ServiceType = orderRequest.ServiceType;
            orderDetail.PerunitPrice = orderRequest.TotalPrice;
            orderDetail.Quantity = orderRequest.Quantity;
            orderDetail.OrderId = order.OrderId;
            orderDetail.VendorId = orderRequest.VendorId;
            orderDetail.Status = "Active";
            orderDetail.UpdatedDate = DateTime.Now;
            orderDetail.UpdatedBy = user.UserId;
            orderDetail.Isdeal = true;
            orderdetailsServices.SaveOrderDetail(orderDetail);


            EventInformation eventInformation = new EventInformation();
            eventInformation.EventName = orderRequest.EventName;
            eventInformation.Email = orderRequest.Email;
            eventInformation.Address = orderRequest.Address;
            eventInformation.Location = orderRequest.Location;
            eventInformation.Phone = orderRequest.Phone;
            eventInformation.PostalCode = orderRequest.PostalCode;
            eventInformation.State = orderRequest.State;
            eventInformation.City = orderRequest.City;
            eventInformation.OrderId = order.OrderId;
            eventInformation.OrderDetailsid = orderDetail.OrderDetailId;

            EventsService eventsService = new EventsService();
            eventInformation = eventsService.SaveEventinformation(eventInformation);

            EventDatesServices eventDatesServices = new EventDatesServices();
            EventDate eventDate = new EventDate();
            foreach (var item in orderRequest.EventDates)
            {
                eventDate.StartDate = item.StartDate;
                eventDate.StartTime = item.StartTime;
                eventDate.EndDate = item.EndDate;
                eventDate.EndTime = item.EndTime;
                eventDate.EventId = eventInformation.EventId;
                string message3 = eventDatesServices.SaveEventDates(eventDate);
            }

            return Json(orderDetail.OrderId);
        }
        [Authorize]
        public JsonResult getproductfromcart(string cid)
        {

            CartService cartService = new CartService();
            var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
            GetCartItems_Result cartlist = cartService.editcartitem(int.Parse(user.UserId.ToString()), int.Parse(cid));
            return Json(cartlist);
        }
        [Authorize]
        public JsonResult Updatecartitem(OrderRequest orderRequest)
        {

            var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
            CartItem cartItem = new CartItem();
            cartItem.VendorId = orderRequest.VendorId;
            cartItem.ServiceType = orderRequest.ServiceType;
            cartItem.Perunitprice = orderRequest.Perunitprice;
            cartItem.TotalPrice = orderRequest.TotalPrice;
            cartItem.Orderedby = user.UserId;
            cartItem.Quantity = orderRequest.Quantity;
            cartItem.UpdatedDate = DateTime.Now;
            cartItem.attribute = orderRequest.attribute;
            cartItem.CartId = orderRequest.cid;

            CartService cartService = new CartService();
            string mesaage = cartService.Updatecartitem(cartItem);
            return Json(mesaage);
        }
    }
}