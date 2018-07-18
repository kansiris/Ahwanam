//using MaaAahwanam.Web.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaaAahwanam.Models;
using MaaAahwanam.Utility;
using System.Configuration;
using System.Web.Security;
//using MaaAahwanam.Repository;
using System.Text;
using System.IO;
using MaaAahwanam.Service;

namespace MaaAahwanam.Web.Areas.Admin.Controllers
{
    public class DashBoardController : Controller
    {
        AdminDashboardService dashboardService = new AdminDashboardService();
        OrderService orderService = new OrderService();
        OthersService othersService = new OthersService();
        QuotationListsService quotationListsService = new QuotationListsService();
        public ActionResult dashboard(string id)
        {
            ViewBag.vendorcount = dashboardService.VendorsCountService();
            ViewBag.commentscount = dashboardService.CommentsCountService();
            ViewBag.ticketcount = dashboardService.TicketsCountService();
            ViewBag.orderscount = dashboardService.OrdersCountService();
            ViewBag.orders = orderService.OrderList().OrderByDescending(m=>m.OrderId).Take(10);
            ViewBag.users = othersService.AllRegisteredUsersDetails().OrderByDescending(m=>m.UserLoginId).Take(4);
            ViewBag.notificationcount = othersService.Notifications().Count();
            ViewBag.quotations = quotationListsService.GetAllQuotations().Where(m => m.Status == "Active").Count();
            //UserDetail userdetail = dashboardService.AdminNameService(long.Parse(id));
            //ViewBag.admin = userdetail.FirstName + " " + userdetail.LastName;
            ViewBag.admin = "admin";
            return View();
        }
    }
}