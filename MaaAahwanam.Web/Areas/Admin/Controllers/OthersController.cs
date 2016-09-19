using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaaAahwanam.Service;
using MaaAahwanam.Models;
using MaaAahwanam.Utility;
using System.IO;
using MaaAahwanam.Repository;
using MaaAahwanam.Web.Custom;

namespace MaaAahwanam.Web.Areas.Admin.Controllers
{
    public class OthersController : Controller
    {
        
        OthersService othersService = new OthersService();
        //
        // GET: /Admin/Others/
        public ActionResult Tickets()
        {
            ViewBag.Tickets = othersService.TicketList();
            return View();
        }
        public ActionResult Comments()
        {
            ViewBag.CommentList = othersService.CommentList();
            return View();
        }
        public ActionResult Testimonials()
        {
            ViewBag.TestimonalsList = othersService.TestimonalsList();
            return View();
        }
        public ActionResult TicketDetails(string id,string Command,IssueDetail issueDetail)
        {
            if (id!=null)
            {
                ViewBag.record = othersService.TicketRecordService(long.Parse(id));
                ViewBag.ticket = othersService.TicketDetail(long.Parse(id));
            }
            if (Command == "Submit")
            {
                issueDetail.TicketId = long.Parse(id);
                issueDetail.RepliedBy = ValidUserUtility.ValidUser();
                issueDetail.ReplayedDate = DateTime.Now;
                issueDetail.UpdatedBy = ValidUserUtility.ValidUser();
                issueDetail = othersService.AddTicket(issueDetail);
                if (issueDetail.TicketCommuId != 0)
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Replied Successfully');location.href='" + @Url.Action("tickets", "others") + "'</script>");
                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Failed');location.href='" + @Url.Action("tickets", "others") + "'</script>");
                }
            }
            return View();
        }
        public ActionResult TestimonialDetails(string id)
        {
            List<MaaAahwanam_Others_TestimonialDetail_Result> testimonal = othersService.TestimonalDetail(long.Parse(id));
            string[] imagenameslist = testimonal[0].ImagePath.Replace(" ", "").Split(',');
            ViewBag.Testimonal = othersService.TestimonalDetail(long.Parse(id));
            List<string> testimonialimages = new List<string>();
            for (int i = 0; i < imagenameslist.Length; i++)
            {
                testimonialimages.Add(imagenameslist[i]);
            }
            ViewBag.images = testimonialimages;
            return View();
        }
        [HttpPost]
        public ActionResult TestimonialDetails(string id,string command)
        {
            AdminTestimonialPath adminTestimonialPath = new AdminTestimonialPath();
            AdminTestimonial adminTestimonial = new AdminTestimonial();
            if (command == "Approve")
            {
                adminTestimonial.Status = "Active";
                adminTestimonialPath.Status = "Active";
                adminTestimonial = othersService.AdminTestimonialStatus(long.Parse(id), adminTestimonial);
                adminTestimonialPath = othersService.AdminTestimonialPathStatus(long.Parse(id), adminTestimonialPath);
                if (adminTestimonial.Id != 0 && adminTestimonialPath.Id!=0)
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Status Updated');location.href='" + @Url.Action("Testimonials", "others") + "'</script>");
                }
                
                    return Content("<script language='javascript' type='text/javascript'>alert('Failed');location.href='" + @Url.Action("Testimonials", "others") + "'</script>");
                
            }
            if (command== "Cancel")
            {
                adminTestimonial.Status = "InActive";
                adminTestimonialPath.Status = "InActive";
                adminTestimonial = othersService.AdminTestimonialStatus(long.Parse(id), adminTestimonial);
                adminTestimonialPath = othersService.AdminTestimonialPathStatus(long.Parse(id), adminTestimonialPath);
                if (adminTestimonial.Id != 0 && adminTestimonialPath.Id != 0)
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Status Updated');location.href='" + @Url.Action("Testimonials", "others") + "'</script>");
                }
                
                    return Content("<script language='javascript' type='text/javascript'>alert('Failed');location.href='" + @Url.Action("Testimonials", "others") + "'</script>");
                
            }
            return View();
        }
        public ActionResult CommentDetails(string id,string uid,string date, CommentDetail commentDetail,string Command)
        {
            
            if (id!=null)
            {
               ViewBag.record = othersService.CommentRecordService(long.Parse(id));
               ViewBag.comment = othersService.CommentDetail(long.Parse(id));
               //return View();
            }
            if (Command == "Submit")
            {
                //var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
                commentDetail.CommentId = long.Parse(id);
                commentDetail.UserLoginId = ValidUserUtility.ValidUser();//(int)user.UserId;
                commentDetail.CommentDate = Convert.ToDateTime(date);
                commentDetail.UpdatedBy = ValidUserUtility.ValidUser();//user.UserId;
                othersService.AddComment(commentDetail);
                if (commentDetail.CommentDetId != 0)
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Replied Successfully');location.href='" + @Url.Action("comments", "others") + "'</script>");
                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Failed');location.href='" + @Url.Action("comments", "others") + "'</script>");
                }
            }
            return View();
        }
        public ActionResult RegisteredUsers()
        {
            ViewBag.users = othersService.RegisteredUsersList();
            return View();
        }
        public ActionResult RegUserDetails(string id)
        {
            if (id != null)
            {
                ViewBag.userdetail = othersService.RegisteredUsersDetails(long.Parse(id));
            }
            return View();
        }

        public ActionResult Notification()
        {
            ViewBag.notification = othersService.Notifications().OrderByDescending(m=>m.DateandTime);
            return View();
        }
	}
}