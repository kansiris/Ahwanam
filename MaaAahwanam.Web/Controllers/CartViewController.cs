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

            foreach (var item in orderRequest.OrderDetail)
            {
                orderDetail.OrderId = order.OrderId;
                orderDetail.OrderBy = user.UserId;
                orderDetail.PerunitPrice = item.PerunitPrice;
                orderDetail.PaymentId = payment_Orders.PaymentID;
                orderDetail.ServiceType = orderRequest.ServiceType;
                orderDetail.ServicePrice = orderRequest.TotalPrice;
                orderDetail.OrderId = order.OrderId;
                orderDetail.VendorId = item.VendorId;
                orderdetailsServices.SaveOrderDetail(orderDetail);
            }

            return Json(orderDetail.OrderId);
        }
    }
}