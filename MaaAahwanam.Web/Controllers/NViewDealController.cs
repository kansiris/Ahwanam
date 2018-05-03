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

namespace MaaAahwanam.Web.Controllers
{
    public class NViewDealController : Controller
    {
        VendorProductsService vendorProductsService = new VendorProductsService();

        // GET: NViewDeal
        public ActionResult Index(string id, string type, string eve)
        {
            try
            {

                //                ViewBag.singledeal = vendorProductsService.getparticulardeal(Int32.Parse(id), type).FirstOrDefault();
                ViewBag.singledeal = vendorProductsService.getpartvendordeal(id, type).FirstOrDefault();
                if (eve != "")
                {
                    var data = vendorProductsService.getpartvendordeal(id, type).Where(m => m.Category == eve);
                    ViewBag.singledeal1 = data ;
                    ViewBag.events = data.Select(m => m.Category).Distinct();

                }
                else
                {
                    var data = vendorProductsService.getpartvendordeal(id, type);
                    ViewBag.singledeal1 = data;
                    ViewBag.events = data.Select(m => m.Category).Distinct();
                }

            return View();
        }
            catch (Exception ex)
            { return RedirectToAction("Index", "Nhomepage");
    }
}

        public PartialViewResult Loadmoredeals(string lastrecord)
        {
            int id = (lastrecord == null) ? 2 : int.Parse(lastrecord) + 2;
            ViewBag.deal = vendorProductsService.getalldeal().OrderBy(m => m.DealID).Take(id);
            var deals = vendorProductsService.getalldeal().OrderBy(m => m.DealID).Take(id);
            ViewBag.dealLastRecord = id;
            ViewBag.dealcount = vendorProductsService.getalldeal().Count();
            return PartialView("Loadmoredeals");
        }

        public ActionResult booknow(string type, string etype1, string date, string totalprice, string id,string price, string guest, string timeslot,string vid, string did)
     {

            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                //  var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
                //  var vendor = vendorProductsService.getparticulardeal(Int32.Parse(id), type).FirstOrDefault();
                //  string updateddate = DateTime.UtcNow.ToShortDateString();
                //  CartItem cartItem = new CartItem();
                //  cartItem.VendorId = vendor.Id;
                //  cartItem.ServiceType = etype1;
                //  cartItem.TotalPrice = decimal.Parse(totalprice);
                //  cartItem.Orderedby = user.UserId;
                //  cartItem.UpdatedDate = Convert.ToDateTime(updateddate);
                //  cartItem.Perunitprice = decimal.Parse(price);
                //  cartItem.Quantity = Convert.ToInt16(guest);
                //  cartItem.subid = vendor.subid;
                ////  cartItem.attribute = orderRequest.attribute;
                //  cartItem.DealId = Convert.ToInt64(id);
                //  CartService cartService = new CartService();
                //  cartItem = cartService.AddCartItem(cartItem);
                var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
                string updateddate = DateTime.UtcNow.ToShortDateString();

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
                return Json("Success", JsonRequestBehavior.AllowGet);
            }
            return Json(JsonRequestBehavior.AllowGet);
        }
        public ActionResult addcnow(string type, string etype1, string date, string totalprice, string id, string price, string guest, string timeslot, string vid, string did)
        {

            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
                var vendor = vendorProductsService.getparticulardeal(Int32.Parse(id), type).FirstOrDefault();
                string updateddate = DateTime.UtcNow.ToShortDateString();
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
            var data = vendorProductsService.getpartvendordeal(id, type).Where(m => m.Category == eve);
            var message = String.Join("~", data.Select(m => new  {   m.DealPrice,  m.FoodType, m.DealID }));
            return Json(message,JsonRequestBehavior.AllowGet);
        }

    }
}