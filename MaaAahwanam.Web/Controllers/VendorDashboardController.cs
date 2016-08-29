using MaaAahwanam.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MaaAahwanam.Web.Controllers
{
    public class VendorDashboardController : Controller
    {
        //
        // GET: /VendorDashboard/
        public ActionResult Index()
        {
            ServiceRequestService serviceRequestService = new ServiceRequestService();
            int bidlistcount = serviceRequestService.GetServiceRequestListcount("Bidding");
            int reversebiddinscount = serviceRequestService.GetServiceRequestListcount("ReverseBidding");
            int quotationcount = serviceRequestService.GetServiceRequestListcount("Quotation");
            ViewBag.biddingcount = bidlistcount;
            ViewBag.reversecount = reversebiddinscount;
            ViewBag.quotationcount = quotationcount;
            return View();
        }
	}
}