using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaaAahwanam.Models;
using MaaAahwanam.Service;
using MaaAahwanam.Web.Custom;
using MaaAahwanam.Repository;

namespace MaaAahwanam.Web.Controllers
{
    public class ReverseBiddingController : Controller
    {
        //
        // GET: /ReverseBidding/
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [Authorize]
        public ActionResult Index(ServiceRequest serviceRequest)
        {
            var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
            ServiceRequestService serviceRequestService = new ServiceRequestService();
            serviceRequest.Type = "ReverseBidding";
            serviceRequest.UpdatedTime = DateTime.Now;
            serviceRequest.Status = "Due";
            serviceRequest.UpdatedBy = user.UserId;
            serviceRequest =serviceRequestService.SaveService(serviceRequest);
            return RedirectToAction("Index", "ReverseBiddingConfirmation", serviceRequest);
        }
        public JsonResult Vendorlist(string selectedservice,string selectedtype)
        {
            ServiceRequestService serviceRequestService = new ServiceRequestService();
            var vlist = serviceRequestService.getvendorslistRB(selectedservice, selectedtype);
            return Json(vlist);
        }
        public JsonResult SubVendorlist(string selectedservice)
        {
            ServiceRequestService serviceRequestService = new ServiceRequestService();
            var subvlist = serviceRequestService.getSubvendorslistRB(selectedservice).GroupBy(s=>s.vendortype).Select(vendortype=> vendortype.First());
            return Json(subvlist);
        }
    }
}