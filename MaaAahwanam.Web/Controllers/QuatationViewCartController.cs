using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaaAahwanam.Service;
using MaaAahwanam.Models;
using MaaAahwanam.Utility;
using MaaAahwanam.Web.Custom;

namespace MaaAahwanam.Web.Controllers
{
    public class QuatationViewCartController : Controller
    {
        DashBoardService dashBoardService = new DashBoardService();
        static long pid;
        static string serviceid;
        public ActionResult Index(string id,string rid)
        {
            if (id != null)
            {
                ViewBag.service = dashBoardService.GetServiceDetailService(long.Parse(id));
                ViewBag.comments = dashBoardService.GetServiceComments(long.Parse(id));
                ViewBag.commentscount = dashBoardService.GetServiceComments(long.Parse(id)).Count;
                ViewBag.id = id;
                serviceid = id;
            }
            if ( rid != null)
            {
                ViewBag.id = serviceid;
                ViewBag.rid = rid;
                ViewBag.service = dashBoardService.GetServiceDetailService(long.Parse(serviceid));
                ViewBag.comments = dashBoardService.GetQuotationComments(long.Parse(rid));
                ViewBag.commentscount = dashBoardService.GetQuotationComments(long.Parse(rid)).Count;
            }
            return View();
        }
        
        [HttpPost]
        public ActionResult Index(string id, string command, CommentDetail commentDetail, Comment comment,string rid)
        {
            if (command == "Submit")
            {
                var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
                comment.ServiceId = id;
                comment.ServiceType = dashBoardService.GetServiceType(long.Parse(id));
                comment.UpdatedBy = (int)user.UserId;
                long count = dashBoardService.GetCommentService(rid, comment.ServiceType);
                
                if (count==0)
                {
                    if (comment.ServiceType == "Quotation")
                    {
                        comment.ServiceId = rid;
                        comment = dashBoardService.InsertCommentService(comment);
                        commentDetail.CommentId = dashBoardService.GetQuotationCommentId(rid); //comment.CommentId;
                        commentDetail.UserLoginId = (int)user.UserId;
                        commentDetail.UpdatedBy = (int)user.UserId;
                        commentDetail = dashBoardService.InsertCommentDetailService(commentDetail);
                        if (commentDetail.CommentDetId != 0)
                        {
                            return Content("<script language='javascript' type='text/javascript'>alert('Comment Uploaded');location.href='" + @Url.Action("index", "QuatationViewCart",new { id = "", rid=rid}) + "'</script>");
                        }
                        return Content("<script language='javascript' type='text/javascript'>alert('Failed!!!');location.href='" + @Url.Action("index", "QuatationViewCart", new { id = "", rid = rid }) + "'</script>");
                    }
                    else
                    {
                        comment = dashBoardService.InsertCommentService(comment);
                        commentDetail.CommentId = dashBoardService.GetCommentId(id); //comment.CommentId;
                        commentDetail.UserLoginId = (int)user.UserId;
                        commentDetail.UpdatedBy = (int)user.UserId;
                        commentDetail = dashBoardService.InsertCommentDetailService(commentDetail);
                        if (commentDetail.CommentDetId != 0)
                        {
                            return Content("<script language='javascript' type='text/javascript'>alert('Comment Uploaded');location.href='" + @Url.Action("index", "QuatationViewCart", new { id = id, rid = "" }) + "'</script>");
                        }
                        return Content("<script language='javascript' type='text/javascript'>alert('Failed!!!');location.href='" + @Url.Action("index", "QuatationViewCart", new { id = id, rid = "" }) + "'</script>");
                    }
                }
                //if (comment.CommentId != 0)
                //{
                    
                //}
            }
            if (command == "Pay")
            {
                OrdersServiceRequest ordersServiceRequest = new OrdersServiceRequest();
                Payments_Requests payments_Requests = new Payments_Requests();
                ServiceResponse serviceResponse = dashBoardService.GetService(long.Parse(serviceid));
                //OrdersServiceRequest record insertion
                ordersServiceRequest.ResponseId = serviceResponse.ResponseId;
                ordersServiceRequest.TotalPrice = serviceResponse.Amount;
                ordersServiceRequest.UpdatedBy = ValidUserUtility.ValidUser();
                ordersServiceRequest.UpdatedDate = DateTime.Now;
                ordersServiceRequest.Status = "Active";
                ordersServiceRequest = dashBoardService.InsertNewOrderService(ordersServiceRequest);
                pid = ordersServiceRequest.ResponseId;
                //Payments_Requests record insertion
                payments_Requests.RequestID = ordersServiceRequest.ResponseId;
                payments_Requests.paidamount = ordersServiceRequest.TotalPrice;
                payments_Requests.cardnumber = "2222 2222 2222 2222";
                payments_Requests.CVV = "234";
                payments_Requests.Paiddate = DateTime.Now;
                payments_Requests = dashBoardService.AddNewPaymentRequest(payments_Requests);
                //Updating Payment ID in OrdersServiceRequest table               
                ordersServiceRequest = dashBoardService.UpdateOrdersServiceRequest(pid, ordersServiceRequest);
                //if (ordersServiceRequest.ResponseId != 0 && payments_Requests.PaymentID != 0)
                //{
                //    return View("confirmation");
                //}
                
                
            }
            return View("confirmation",serviceid);
        }

        public ActionResult confirmation(string id)
        {
            ServiceRequest serviceRequest = dashBoardService.UpdateService(long.Parse(id));
            return View("confirmation");
        }
    }
}