using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaaAahwanam.Service;

namespace MaaAahwanam.Web.Controllers
{
    public class NVendorOrdersController : Controller
    {
        OrderService orderService = new OrderService();
        // GET: NVendorOrders
        public ActionResult Index(string id)
        {
            ViewBag.id = id;
            var orders = orderService.userOrderList().Where(m => m.Id == int.Parse(id));
            ViewBag.order = orders.OrderByDescending(m => m.OrderId);
            return View();
        }
    }
}