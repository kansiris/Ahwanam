using MaaAahwanam.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MaaAahwanam.Web.Areas.Admin.Controllers
{

    public class quotationlistController : Controller
    {
        QuotationListsService quotationListsService = new QuotationListsService();

        // GET: Admin/quotationlist
        public ActionResult Index()
        {
            ViewBag.quotations = quotationListsService.GetAllQuotations().ToList(); //.Where(m => m.Status == "Active")
            return View();
        }
    }
}