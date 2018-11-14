using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaaAahwanam.Service;
using MaaAahwanam.Repository;
using MaaAahwanam.Utility;
using System.IO;
using MaaAahwanam.Models;

namespace MaaAahwanam.Web.Areas.Admin.Controllers
{
    public class QuotationsController : Controller
    {

        public ActionResult Index()
        {

            return View();
        }

    }
}