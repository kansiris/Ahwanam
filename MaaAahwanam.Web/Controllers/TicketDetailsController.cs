using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaaAahwanam.Models;
using MaaAahwanam.Service;
using MaaAahwanam.Web.Custom;

namespace MaaAahwanam.Web.Controllers
{
    public class TicketDetailsController : Controller
    {
        ticketsService _ticketsService = new ticketsService();
        // GET: TicketDetails
        public ActionResult Index()
        {
            string TicketID = Request.QueryString["tid"];
            ViewBag.TicketID = TicketID;
            ViewBag.CommentDetails=_ticketsService.GetTicketsdetails(Convert.ToInt32(TicketID));
            return View();
        }
        [HttpPost]
        public ActionResult Index(IssueDetail issueDetail)
        {
            var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
            string updateddate = DateTime.UtcNow.ToShortDateString();
            issueDetail.UpdatedDate = Convert.ToDateTime(updateddate);
            issueDetail.UpdatedBy =user.UserId;
            _ticketsService.InsertIssueDetail(issueDetail);
            return Content("<script language='javascript' type='text/javascript'>alert('Submitted successfully!');location.href='" + @Url.Action("Index", "TicketDetails",new { tid= issueDetail.TicketId}) + "'</script>");
            //return View();
        }
    }
}