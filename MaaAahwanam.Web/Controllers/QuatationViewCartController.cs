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
        public ActionResult Index(string id)
        {
            if (id != null)
            {
                ViewBag.service = dashBoardService.GetServiceDetailService(long.Parse(id));
                ViewBag.comments = dashBoardService.GetServiceComments(long.Parse(id));
                ViewBag.commentscount = dashBoardService.GetServiceComments(long.Parse(id)).Count;
                ViewBag.id = id;
            }
            return View();
        }

        [HttpPost]
        public ActionResult Index(string id, string command, CommentDetail commentDetail, Comment comment)
        {
            if (command == "Submit")
            {
                var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
                comment.ServiceId = id;
                comment.ServiceType = dashBoardService.GetServiceType(long.Parse(id));
                comment.UpdatedBy = (int)user.UserId;
                comment = dashBoardService.InsertCommentService(comment);
                if (comment.CommentId != 0)
                {
                    commentDetail.CommentId = comment.CommentId;//dashBoardService.GetCommentId(id);
                    commentDetail.UserLoginId = (int)user.UserId;
                    commentDetail.UpdatedBy = (int)user.UserId;
                    commentDetail = dashBoardService.InsertCommentDetailService(commentDetail);
                    if (commentDetail.CommentDetId != 0)
                    {
                        return Content("<script language='javascript' type='text/javascript'>alert('Comment Uploaded');location.href='" + @Url.Action("index", "QuatationViewCart") + "'</script>");
                    }
                    return Content("<script language='javascript' type='text/javascript'>alert('Failed!!!');location.href='" + @Url.Action("index", "QuatationViewCart") + "'</script>");
                }
            }
            return View();
        }
    }
}