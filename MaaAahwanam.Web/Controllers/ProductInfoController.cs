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
    public class ProductInfoController : Controller
    {
        ReviewService reviewService = new ReviewService();
        //
        // GET: /CardInfo/
        public ActionResult Index()
        {

            ProductInfoService productInfoService = new ProductInfoService();
            Review review = new Review();
            string Servicetype = Request.QueryString["par"];
            int vid = Convert.ToInt32(Request.QueryString["VID"]);
            GetProductsInfo_Result Productinfo = productInfoService.getProductsInfo_Result(vid, Servicetype);
            if(Productinfo.image!=null)
            { 
            string[] imagenameslist = Productinfo.image.Replace(" ", "").Split(',');
            ViewBag.Imagelist = imagenameslist;
            }
            ViewBag.servicetype = Servicetype;
            ViewBag.Reviewlist = reviewService.GetReview(vid);

            var tupleModel = new Tuple<GetProductsInfo_Result, Review>(Productinfo, review);
            return View(tupleModel);
        }
        public ActionResult WriteaRiview([Bind(Prefix = "Item2")] Review review)
        {
            int a = ValidUserUtility.ValidUser();
            if (ValidUserUtility.ValidUser() != 0 && (ValidUserUtility.UserType() == "User"))
            {
                review.UpdatedBy = ValidUserUtility.ValidUser();
                review.Status = "Active";
                review.UpdatedDate = DateTime.Now;
                reviewService.InsertReview(review);
                return RedirectToAction("Index", new { par = review.Service, VID = review.ServiceId });
            }
            return RedirectToAction("Index", "Signin");
        }

        public JsonResult Addtocart(OrderRequest orderRequest)
        {
            var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
            CartItem cartItem = new CartItem();
            cartItem.VendorId = orderRequest.VendorId;
            cartItem.ServiceType = orderRequest.ServiceType;
            cartItem.TotalPrice = orderRequest.TotalPrice;
            cartItem.Orderedby = user.UserId;
            cartItem.UpdatedDate = DateTime.Now;

            EventInformation eventInformation = new EventInformation();
            eventInformation.EventName = orderRequest.EventName;
            eventInformation.Email = orderRequest.Email;
            eventInformation.Address = orderRequest.Address;
            eventInformation.Location = orderRequest.Location;
            eventInformation.Phone = orderRequest.Phone;
            eventInformation.PostalCode = orderRequest.PostalCode;
            eventInformation.State = orderRequest.State;
            eventInformation.City = orderRequest.City;

            EventDate eventDate = new EventDate();
            foreach (var item in orderRequest.EventDates)
            {
                eventDate.StartDate = item.StartDate;
                eventDate.StartTime = item.StartTime;
                eventDate.EndDate = item.EndDate;
                eventDate.EndTime = item.EndTime;
            }
            CartService cartService = new CartService();
            string mesaage = cartService.AddCartItem(cartItem);
            EventsService eventsService = new EventsService();
            string mesaage1 = eventsService.SaveEventinformation(eventInformation);
            EventDatesServices eventDatesServices = new EventDatesServices();
            string message3 = eventDatesServices.SaveEventDates(eventDate);
            return Json(mesaage);
        }

        public JsonResult Buynow(OrderRequest orderRequest)
        {
            var user = (CustomPrincipal)System.Web.HttpContext.Current.User;

            EventInformation eventInformation = new EventInformation();
            eventInformation.EventName = orderRequest.EventName;
            eventInformation.Email = orderRequest.Email;
            eventInformation.Address = orderRequest.Address;
            eventInformation.Location = orderRequest.Location;
            eventInformation.Phone = orderRequest.Phone;
            eventInformation.PostalCode = orderRequest.PostalCode;
            eventInformation.State = orderRequest.State;
            eventInformation.City = orderRequest.City;

            EventDatesServices eventDatesServices = new EventDatesServices();
            EventDate eventDate = new EventDate();
            foreach (var item in orderRequest.EventDates)
            {
                eventDate.StartDate = item.StartDate;
                eventDate.StartTime = item.StartTime;
                eventDate.EndDate = item.EndDate;
                eventDate.EndTime = item.EndTime;
                string message3 = eventDatesServices.SaveEventDates(eventDate);
            }
            EventsService eventsService = new EventsService();
            string mesaage1 = eventsService.SaveEventinformation(eventInformation);

            OrderService orderService = new OrderService();
            Order order = new Order();
            order.TotalPrice = orderRequest.TotalPrice;
            order.OrderDate = DateTime.Now;
            order.UpdatedBy = user.UserId;
            order = orderService.SaveOrder(order);

            Payment_orderServices payment_orderServices = new Payment_orderServices();
            Payment_Orders payment_Orders = new Payment_Orders();
            payment_Orders.cardnumber = orderRequest.cardnumber;
            payment_Orders.CVV = orderRequest.CVV;
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
            orderDetail.OrderId = order.OrderId;
            orderDetail.VendorId = orderRequest.VendorId;
            orderdetailsServices.SaveOrderDetail(orderDetail);

            return Json(orderDetail.OrderId);
        }
    }
}