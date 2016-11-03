using MaaAahwanam.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaaAahwanam.Service;
using MaaAahwanam.Utility;
using MaaAahwanam.Web.Custom;
using MaaAahwanam.Models;

namespace MaaAahwanam.Web.Controllers
{
    public class CartViewController : Controller
    {
        CartService cartService = new CartService();
        //
        // GET: /CartView/
        [Authorize]
        public ActionResult Index()
        {
            var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
            List<GetCartItems_Result> cartlist = cartService.CartItemsList(int.Parse(user.UserId.ToString()));
            decimal total = cartlist.Sum(s => s.TotalPrice);
            ViewBag.Cartlist = cartlist;
            ViewBag.Total = total;
            return View();
        }
        public JsonResult payamount(OrderRequest orderRequest)
        {
            var user = (CustomPrincipal)System.Web.HttpContext.Current.User;

            OrderService orderService = new OrderService();
            Order order = new Order();
            order.TotalPrice = orderRequest.TotalPrice;
            order.OrderDate = DateTime.Now;
            order.UpdatedBy = user.UserId;
            order.OrderedBy = user.UserId;
            order.UpdatedDate = DateTime.Now;
            order.Status = "Active";
            //order.PaymentId = payment_Orders.OrderID;
            order = orderService.SaveOrder(order);
            EventsService eventsService = new EventsService();
            foreach (var item2 in orderRequest.Cartitems)
            {
                eventsService.updateeventid(item2.CartId, order.OrderId);
            }
            Payment_orderServices payment_orderServices = new Payment_orderServices();
            Payment_Orders payment_Orders = new Payment_Orders();
            payment_Orders.cardnumber = orderRequest.cardnumber;
            payment_Orders.CVV = orderRequest.CVV;
            payment_Orders.PaymentID = orderRequest.PaymentId;
            payment_Orders.Paiddate = orderRequest.Paiddate;
            payment_Orders.paidamount = orderRequest.TotalPrice;
            payment_Orders.OrderID = order.OrderId;
            //payment_Orders.OrderID = order.OrderId;
            payment_Orders = payment_orderServices.SavePayment_Orders(payment_Orders);

            OrderdetailsServices orderdetailsServices = new OrderdetailsServices();
            OrderDetail orderDetail = new OrderDetail();
            int i = 0;
            foreach (var item in orderRequest.OrderDetail)
            {
                orderDetail.OrderId = order.OrderId;
                orderDetail.OrderBy = user.UserId;
                orderDetail.PerunitPrice = item.PerunitPrice;
                orderDetail.PaymentId = payment_Orders.PaymentID;
                orderDetail.ServiceType = orderRequest.ServiceType;
                orderDetail.TotalPrice = orderRequest.TotalPrice;
                orderDetail.OrderId = order.OrderId;
                orderDetail.VendorId = item.VendorId;
                orderDetail.subid = item.subid;
                orderDetail.Status = "Active";
                orderDetail.UpdatedDate = DateTime.Now;
                orderDetail.UpdatedBy = user.UserId;
                
                if (i == 0)
                {
                    orderDetail.Isdeal = orderRequest.Cartitems[0].Isdeal;
                    orderDetail.DealId = orderRequest.Cartitems[0].DealId;
                    orderDetail = orderdetailsServices.SaveOrderDetail(orderDetail);
                    eventsService.updateeventodid(orderRequest.Cartitems[0].CartId,orderDetail.OrderDetailId);
                }
                else
                {
                    orderDetail.Isdeal = orderRequest.Cartitems[i].Isdeal;
                    orderDetail.DealId = orderRequest.Cartitems[i].DealId;
                    orderDetail = orderdetailsServices.SaveOrderDetail(orderDetail);
                    eventsService.updateeventodid(orderRequest.Cartitems[i].CartId, orderDetail.OrderDetailId);
                }
                i++;
            }
            foreach (var item1 in orderRequest.Cartitems)
            {
                cartService.Deletecartitem(item1.CartId);
            }
            return Json(orderDetail.OrderId);
        }
        public JsonResult DeletecartItem(long cartId)
        {
            var message = cartService.Deletecartitem(cartId);
            return Json(message);
        }
    }
}