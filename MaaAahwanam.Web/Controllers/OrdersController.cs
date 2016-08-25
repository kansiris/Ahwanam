using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaaAahwanam.Models;
using MaaAahwanam.Utility;
using System.Configuration;
using System.Web.Security;
using MaaAahwanam.Web.Custom;

namespace MaaAahwanam.Web.Controllers
{
    public class OrdersController : Controller
    {
        //
        // GET: /Orders/
        public ActionResult Index()
        {
            var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
            ViewBag.Type = user.UserType;
            return View();
        }
	}
}