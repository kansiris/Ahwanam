using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaaAahwanam.Service;
using MaaAahwanam.Models;

namespace MaaAahwanam.Web.Controllers
{
    public class VendorBiddingslistController : Controller
    {
        // GET: VendorBiddingslist
        public ActionResult Index(ServiceRequest serviceRequest)
        {
            serviceRequest.Type = "Bidding";
            ServiceRequestService serviceRequestService = new ServiceRequestService();
            List<ServiceRequest> bidlist=serviceRequestService.GetServiceRequestList(serviceRequest);
            ViewBag.bidlist = bidlist.OrderByDescending(m => m.RequestId);
            return View();
        }
        public ActionResult test()
        {
            return View();
        }
    }
}