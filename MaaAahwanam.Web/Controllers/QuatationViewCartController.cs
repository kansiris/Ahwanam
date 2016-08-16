using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaaAahwanam.Service;
using MaaAahwanam.Models;
using MaaAahwanam.Utility;

namespace MaaAahwanam.Web.Controllers
{
    public class QuatationViewCartController : Controller
    {
        DashBoardService dashBoardService = new DashBoardService();
        public ActionResult Index(string id)
        {
            if (id!=null)
            {
                ViewBag.service = dashBoardService.GetServiceDetailService(long.Parse(id));
                ViewBag.comments = dashBoardService.GetServiceComments(long.Parse(id));
                ViewBag.commentscount = dashBoardService.GetServiceComments(long.Parse(id)).Count;
                ViewBag.id = id;
            }
            return View();
        }
        
        [HttpPost]
        public ActionResult Index(string id, string command, CommentDetail commentDetail)
        {
            if (command == "Submit")
            {
                commentDetail.CommentId = dashBoardService.GetCommentId(id);
                commentDetail.UserLoginId = ValidUserUtility.ValidUser();
                commentDetail.UpdatedBy = ValidUserUtility.ValidUser();
                commentDetail = dashBoardService.InsertCommentService(commentDetail);
                if (commentDetail.CommentDetId != 0)
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Comment Uploaded');location.href='" + @Url.Action("index", "QuatationViewCart") + "'</script>");
                }
                return Content("<script language='javascript' type='text/javascript'>alert('Failed!!!');location.href='" + @Url.Action("index", "QuatationViewCart") + "'</script>");
            }
            return View();
        }
    }
}