using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaaAahwanam.Service;
using MaaAahwanam.Models;

namespace MaaAahwanam.Web.Controllers
{
    public class VendorReverseBiddingslistController : Controller
    {
        // GET: VendorReverseBiddingslist
        public ActionResult Index(ServiceRequest serviceRequest)
        {
            serviceRequest.Type = "ReverseBidding";
            ServiceRequestService serviceRequestService = new ServiceRequestService();
            List<ServiceRequest> bidlist = serviceRequestService.GetServiceRequestList(serviceRequest);
            ViewBag.bidlist = bidlist.OrderByDescending(m => m.RequestId);
            return View();
        }
    }
}