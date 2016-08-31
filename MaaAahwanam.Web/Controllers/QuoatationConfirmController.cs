using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaaAahwanam.Models;

namespace MaaAahwanam.Web.Controllers
{
    public class QuoatationConfirmController : Controller
    {
        //
        // GET: /QuoatationConfirm/
        public ActionResult Index(ServiceRequest serviceRequest)
        {
            return View(serviceRequest);
        }
    }
}