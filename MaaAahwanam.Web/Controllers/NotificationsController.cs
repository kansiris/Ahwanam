using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaaAahwanam.Models;
using MaaAahwanam.Utility;
using System.Configuration;
using System.Web.Security;
using MaaAahwanam.Service;
using MaaAahwanam.Web.Custom;

namespace MaaAahwanam.Web.Controllers
{
    [Authorize]
    public class NotificationsController : Controller
    {
        NotificationService notificationService = new NotificationService();
        public ActionResult Index(string id, string type)
        {
            if (type != null)
            {
                Notification notification = notificationService.RemoveNotificationService(long.Parse(id));
            }
            var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
            ViewBag.Type = user.UserType;
            long userid = user.UserId;
            ViewBag.AllNotifications = notificationService.GetNotificationService(userid);
            
            return View();
        }
    }
}