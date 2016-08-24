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


namespace MaaAahwanam.Web.Controllers
{
    public class DashboardController : Controller
    {
        DashBoardService dashBoardService = new DashBoardService();
        public ActionResult Index()
        {
            if (ValidUserUtility.ValidUser() != 0 && (ValidUserUtility.UserType() == "User" || ValidUserUtility.UserType() == "Vendor"))
            {

                ViewBag.Type = ValidUserUtility.UserType();

            }
            int id = ValidUserUtility.ValidUser();
            ViewBag.AllOrders = dashBoardService.GetOrdersService(id);
            ViewBag.Services = dashBoardService.GetServicesService(id);
            return View();
        }
    }
}