using MaaAahwanam.Models;
using MaaAahwanam.Service;
using MaaAahwanam.Web.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MaaAahwanam.Web.Controllers
{
    public class VendorBiddingResponseController : Controller
    {
        ServiceResponseService serviceResponseService = new ServiceResponseService();
        // GET: VendorReverseBiddingResponseController
        public ActionResult Index()
        {
            string Rid = Request.QueryString["Rid"];
            ViewBag.rid = Rid;
            return View();
        }
        [HttpPost]
        public ActionResult Index(ServiceResponse serviceResponse)
        {
            var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
            string updateddate = DateTime.UtcNow.ToShortDateString();
            serviceResponse.Status = "Active";
            serviceResponse.UpdatedBy = user.UserId;
            serviceResponse.ResponseBy = user.UserId;
            serviceResponse.UpdatedDate = Convert.ToDateTime(updateddate);
            string message = serviceResponseService.SaveServiceResponse(serviceResponse);
            
            if (message == "Success")
            //{
                return Content("<script language='javascript' type='text/javascript'>alert('Submitted Response');location.href='" + @Url.Action("Index", "VendorBiddingResponse", new { Rid = serviceResponse.RequestId}) + "'</script>");
            //}
                return Content("<script language='javascript' type='text/javascript'>alert('Failed to submit please try again');location.href='" + @Url.Action("Index", "VendorBiddingResponse", new { Rid = serviceResponse.RequestId }) + "'</script>");
            
        }
    }
}