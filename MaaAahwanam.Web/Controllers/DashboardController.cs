using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaaAahwanam.Repository;
using MaaAahwanam.Models;
using MaaAahwanam.Utility;
using System.Configuration;
using System.Web.Security;
using MaaAahwanam.Service;
using MaaAahwanam.Web.Custom;
using System.IO;
using System.Web.Helpers;

namespace MaaAahwanam.Web.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        NotificationService notificationService = new NotificationService();
        DashBoardService dashBoardService = new DashBoardService();
        public ActionResult Index()
        {
            var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
            ViewBag.Type = user.UserType;
            int id = (int)user.UserId;
            ViewBag.AllOrders = dashBoardService.GetOrdersService(id);
            ViewBag.Services = dashBoardService.GetServicesService(id);
            
            ViewBag.orderscount = dashBoardService.GetOrdersService(id).Count();
            ViewBag.servicescount = dashBoardService.GetServicesService(id).Count();
            ViewBag.notificationcount = notificationService.GetNotificationService(id).Count();
            return View();
        }

        public JsonResult SetUserDP()
        {
            UserLoginDetailsService userLoginDetailsService = new UserLoginDetailsService();
            var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
            int UserloginID = (int)user.UserId;
            string imagename=userLoginDetailsService.SetUserDP(UserloginID);
            if (imagename == null)
                imagename = "user1.jpg";
            return Json(imagename);
        }
        public JsonResult UpdateDP()
        {
            string _imgname = string.Empty;
            var _comPath = "";
            var fileName = string.Empty;
            if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
            {
                var pic = System.Web.HttpContext.Current.Request.Files["MyImages"];
                if (pic.ContentLength > 0)
                {
                    fileName = Path.GetFileName(pic.FileName);
                    var _ext = Path.GetExtension(pic.FileName);

                    //_imgname = Guid.NewGuid().ToString();
                    _comPath = Server.MapPath("../Content/UserDPs/UserDP_") + fileName;
                    _imgname = "UserDP_" + fileName;

                    ViewBag.Msg = _comPath;
                    var path = _comPath;

                    // Saving Image in Original Mode
                    pic.SaveAs(path);

                    // resizing image
                    MemoryStream ms = new MemoryStream();
                    WebImage img = new WebImage(_comPath);

                    if (img.Width > 200)
                        img.Resize(200, 200);
                    img.Save(_comPath);
                    var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
                    var userloignId = (int)user.UserId;
                    UserLoginDetailsService userLoginDetailsService = new UserLoginDetailsService();
                    userLoginDetailsService.ChangeDP(userloignId,_imgname);
                    // end resize
                }
            }
            return Json(_imgname.ToString());
        }

    }
}