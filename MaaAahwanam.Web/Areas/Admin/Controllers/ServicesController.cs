using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaaAahwanam.Service;
using MaaAahwanam.Models;
using MaaAahwanam.Web.Custom;
using MaaAahwanam.Utility;
using System.IO;

namespace MaaAahwanam.Web.Areas.Admin.Controllers
{
    public class ServicesController : Controller
    {
        ServiceRequestService serviceRequestService = new ServiceRequestService();
        ServiceResponseService serviceResponseService = new ServiceResponseService();
        public ActionResult BidRequests(ServiceRequest serviceRequest, string BidReqId, string name, ServiceResponse serviceResponse)
        {
            serviceRequest.Type = "Bidding";
            ViewBag.records = serviceRequestService.GetServiceRequestList(serviceRequest).OrderByDescending(m=>m.RequestId);
            if (name == "View")
            {
                serviceRequest.RequestId = long.Parse(BidReqId);
                serviceResponse.RequestId = long.Parse(BidReqId);
                TempData["ServiceRequestRecords"] = serviceRequestService.GetServiceRequestRecord(serviceRequest); //Bidding
                TempData["ServiceResponseCount"] = serviceResponseService.ServiceResponseCount(serviceResponse);
                return RedirectToAction("BidReqView");
            }
            if (name == "Delete")
            {

            }
            return View();
        }
        public ActionResult QuotRequests(ServiceRequest serviceRequest, string BidReqId, string name, ServiceResponse serviceResponse)
        {
            serviceRequest.Type = "Quotation";
            ViewBag.records = serviceRequestService.GetServiceRequestList(serviceRequest).OrderByDescending(m => m.RequestId);
            if (name == "View")
            {
                serviceRequest.RequestId = long.Parse(BidReqId);
                serviceResponse.RequestId = long.Parse(BidReqId);
                TempData["ServiceRequestRecords"] = serviceRequestService.GetServiceRequestRecord(serviceRequest); //Quotation
                TempData["ServiceResponseCount"] = serviceResponseService.ServiceResponseCount(serviceResponse);
                return RedirectToAction("QuotReqView");
            }
            return View();
        }
        public ActionResult RevBidRequests(ServiceRequest serviceRequest, string BidReqId, string name, ServiceResponse serviceResponse)
        {
            serviceRequest.Type = "ReverseBidding";
            ViewBag.records = serviceRequestService.GetServiceRequestList(serviceRequest).OrderByDescending(m => m.RequestId);
            if (name == "View")
            {
                serviceRequest.RequestId = long.Parse(BidReqId);
                TempData["ServiceRequestRecords"] = serviceRequestService.GetServiceRequestRecord(serviceRequest); //Quotation
                return RedirectToAction("RevBidReqView");
            }
            return View();
        }
        public ActionResult BidReqView()
        {
            //Bidding related
            if (TempData["ServiceRequestRecords"] != null && TempData["ServiceResponseCount"] != null)
            {
                ViewBag.count = TempData["ServiceResponseCount"];
                ViewBag.vendordetails = TempData["ServiceRequestRecords"];
                return View();
            }
            return View();
        }
        public ActionResult QuotReqView()
        {
            if (TempData["ServiceRequestRecords"] != null && TempData["ServiceResponseCount"] != null)
            {
                //Quotation Related
                ViewBag.count = TempData["ServiceResponseCount"];
                ViewBag.vendordetails = TempData["ServiceRequestRecords"];
                return View();
            }
            return View();
        }
        public ActionResult RevBidReqView()
        {
            if (TempData["ServiceRequestRecords"] != null)
            {
                //Reverse Bid Related
                long id = 0;
                ViewBag.vendordetails = TempData["ServiceRequestRecords"];
                foreach (var item in ViewBag.vendordetails)
                {
                    id = item.VendorId;
                }
                ViewBag.vendorname = serviceRequestService.getvendorname(id);
                return View();
            }
            return View();
        }
        public ActionResult Biddings(string id)
        {
            if (id != null)
            {
                ViewBag.ServiceResponseRecordsList = serviceResponseService.GetServiceResponseList(long.Parse(id));
                ViewBag.bidid = id;
                return View();
            }
            return View();
        }
        public ActionResult Quotations(string id, ServiceResponse serviceResponse)
        {
            if (id != null)
            {
                ServiceRequest serviceRequest = new ServiceRequest();
                serviceRequest.RequestId = serviceResponse.RequestId = long.Parse(id);
                ViewBag.QuotationRecordsList = serviceResponseService.GetQuotationList(serviceResponse);
                var list = serviceRequestService.GetServiceRequestRecord(serviceRequest);
                ViewBag.quotdate = list[0].EventStartDate.ToShortDateString();
                ViewBag.quotid = id;
            }
            return View();
        }
        public ActionResult ReverseBiddings()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Quotations(string command, string id, ServiceResponse serviceResponse, string rid)
        {
            var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
            string updateddate = DateTime.UtcNow.ToShortDateString();
            serviceResponse.RequestId = long.Parse(id);
            serviceResponse.Status = "Active";
            serviceResponse.UpdatedDate = Convert.ToDateTime(updateddate);
            serviceResponse.UpdatedBy= serviceResponse.ResponseBy = user.UserId;
            string message = "";
            if (command == "Submit")
            {
                message = serviceResponseService.SaveServiceResponse(serviceResponse);
                if (message == "Success")
                return Content("<script language='javascript' type='text/javascript'>alert('Quotation Replied successfully!');location.href='" + @Url.Action("Quotations", "Services", new { id = serviceResponse.RequestId }) + "'</script>");
            }
            if (command == "Update")
            {
                message = serviceResponseService.UpdateServiceResponse(serviceResponse);
                if (message == "Success")
                return Content("<script language='javascript' type='text/javascript'>alert('Quotation updated successfully!');location.href='" + @Url.Action("Quotations", "Services", new { id = serviceResponse.RequestId }) + "'</script>");
            }
            return Content("<script language='javascript' type='text/javascript'>alert('Failed!!!');location.href='" + @Url.Action("Quotations", "Services") + "'</script>");
        }
    }
}
            
           
        
