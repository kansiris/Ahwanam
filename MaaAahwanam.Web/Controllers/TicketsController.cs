using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaaAahwanam.Models;
using MaaAahwanam.Utility;
using MaaAahwanam.Service;
using MaaAahwanam.Web.Custom;
using System.IO;

namespace MaaAahwanam.Web.Controllers
{
    [Authorize]
    public class TicketsController : Controller
    {
        ticketsService ticketsServices = new ticketsService();
        // GET: /Tickets/
        public ActionResult Index()
        {
            var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
            ViewBag.Type = user.UserType;
            var a = ticketsServices.GetIssueTicket((int)user.UserId);
            ViewBag.Issueticketslist = a;
            return View();
        }

        [HttpPost]
        public ActionResult Index(string name, IssueTicket issueTicket)
        {
            var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
            string updateddate = DateTime.UtcNow.ToShortDateString();
            issueTicket.UpdatedBy = (int)user.UserId;
            issueTicket.Status = "Active";
            issueTicket.UpdatedBy =user.UserId;
            issueTicket.UpdatedDate =Convert.ToDateTime(updateddate);
            issueTicket.UserLoginId = user.UserId;
            string status = ticketsServices.Insertissueticket(issueTicket);
            if (status == "Success")
            {
                return Content("<script language='javascript' type='text/javascript'>alert('Ticket Raised');location.href='" + @Url.Action("Index", "Tickets") + "'</script>");
            }
            else
            {
                return Content("<script language='javascript' type='text/javascript'>alert('Ticket Not Raised');location.href='" + @Url.Action("Index", "Tickets") + "'</script>");
            }
        }
    }
}