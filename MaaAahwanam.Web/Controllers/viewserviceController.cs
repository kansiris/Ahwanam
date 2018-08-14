using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MaaAahwanam.Web.Controllers
{
    public class viewserviceController : Controller
    {
        // GET: viewservice
        public ActionResult Index(string id,string vid)
        {
            return View();
        }
    }
}