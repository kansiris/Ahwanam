using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaaAahwanam.Repository;
using MaaAahwanam.Models;
using MaaAahwanam.Utility;
using System.Configuration;
using System.Web.Security;
using MaaAahwanam.Service;
using MaaAahwanam.Web.Custom;


namespace MaaAahwanam.Web.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        DashBoardService dashBoardService = new DashBoardService();
        public ActionResult Index()
        {
            var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
            ViewBag.Type = user.UserType;            
            int id = (int)user.UserId;
            ViewBag.AllOrders = dashBoardService.GetOrdersService(id);
            ViewBag.Services = dashBoardService.GetServicesService(id);
            return View();
        }
    }
}