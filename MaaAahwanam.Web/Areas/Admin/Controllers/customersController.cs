using MaaAahwanam.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MaaAahwanam.Web.Areas.Admin.Controllers
{
    public class customersController : Controller
    {
        VendorDashBoardService mnguserservice = new VendorDashBoardService();
        // GET: Admin/customers
        public ActionResult Index()
        {
            var customers = mnguserservice.allcustlist1().ToList();
            ViewBag.cust = customers;
            return View();
        }
    }
}