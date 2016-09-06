using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaaAahwanam.Service;
using MaaAahwanam.Models;
using MaaAahwanam.Utility;
using System.IO;

namespace MaaAahwanam.Web.Areas.Admin.Controllers
{
    public class OrdersController : Controller
    {
        OrderService orderservice = new OrderService();
        public ActionResult AllOrders()
        {
            ViewBag.OrdersList = orderservice.OrderList();
            return View();
        }
        public ActionResult OrderDetails(string id)
        {
            if (id!=null)
            {
                ViewBag.OrderDetailsList = orderservice.OrderDetailServivce(long.Parse(id)).OrderByDescending(m => m.UpdatedDate);
                ViewBag.orderid = id;
                return View();
            }
            return View();
        }
	}
}