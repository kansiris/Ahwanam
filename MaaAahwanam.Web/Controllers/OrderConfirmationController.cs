using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaaAahwanam.Models;
using MaaAahwanam.Service;
using MaaAahwanam.Repository;

namespace MaaAahwanam.Web.Controllers
{
    public class OrderConfirmationController : Controller
    {
        // GET: OrderConfirmation
        public ActionResult Index()
        {
            int OID = int.Parse(Request.QueryString["oid"]);
            OrderConfirmationService orderConfirmationService = new OrderConfirmationService();
            List<orderconfirmation_Result> list= orderConfirmationService.GetOrderConfirmation(OID);
            ViewBag.Total =list.Sum(i=>i.PerunitPrice);
            ViewBag.Orderconfirmation = list;
            return View();
        }
    }
}