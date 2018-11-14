using MaaAahwanam.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MaaAahwanam.Web.Areas.Admin.Controllers
{
    public class nordersController : Controller
    {
        OrderService orderservice = new OrderService();

        // GET: Admin/norders
        public ActionResult Index()
        {
            ViewBag.OrdersList = orderservice.OrderList().OrderByDescending(m => m.OrderId);
            return View();
        }
    }
}