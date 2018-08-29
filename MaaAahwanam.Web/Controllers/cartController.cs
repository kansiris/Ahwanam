using MaaAahwanam.Models;
using MaaAahwanam.Service;
using MaaAahwanam.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace MaaAahwanam.Web.Controllers
{
    public class cartController : Controller
    {
        cartservices cartserve = new cartservices();
        // GET: cart
        public ActionResult Index()
        {
            
              
            
            return View();
        }

        public JsonResult cookies(string pid, string guest)
        {

            var data = cartserve.getvendorpkgs(pid);


            if (data != null)
            {
                return Json("success", JsonRequestBehavior.AllowGet);
            }
            return Json("unique", JsonRequestBehavior.AllowGet);
        }

      
    }
}

