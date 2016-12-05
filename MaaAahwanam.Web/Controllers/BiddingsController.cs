using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaaAahwanam.Web.Custom;

namespace MaaAahwanam.Web.Controllers
{
    public class BiddingsController : Controller
    {
        //
        // GET: /Biddings/
        public ActionResult Index()
        {
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
                ViewBag.Userloginstatus = user.UserId;
            }
            else
            {
                ViewBag.Userloginstatus = 0;
            }
            return View();
        }
	}
}