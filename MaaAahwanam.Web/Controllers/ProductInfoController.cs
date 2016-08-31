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
            if (Productinfo.image != null)
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
            var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
            review.UpdatedBy = (int)user.UserId;
            review.Status = "Active";
            review.UpdatedDate = DateTime.Now;
            reviewService.InsertReview(review);
            return RedirectToAction("Index", new { par = review.Service, VID = review.ServiceId });

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
            cartItem.attribute = orderRequest.attribute;
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
            orderDetail.OrderId = order.OrderId;
            orderDetail.VendorId = orderRequest.VendorId;
            orderDetail.Status = "Active";
            orderDetail.UpdatedDate = DateTime.Now;
            orderDetail.UpdatedBy = user.UserId;
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

        public JsonResult getproductfromcart(string cid)
        {

            CartService cartService = new CartService();
            var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
            GetCartItems_Result cartlist = cartService.editcartitem(int.Parse(user.UserId.ToString()), int.Parse(cid));
            return Json(cartlist);
        }
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