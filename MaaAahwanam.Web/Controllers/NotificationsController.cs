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

namespace MaaAahwanam.Web.Controllers
{
    public class NotificationsController : Controller
    {
        NotificationService notificationService = new NotificationService();
        public ActionResult Index(string id,string type)
        {
            if (ValidUserUtility.ValidUser() != 0 && (ValidUserUtility.UserType() == "User" || ValidUserUtility.UserType() == "Vendor"))
            {
                ViewBag.Type = ValidUserUtility.UserType();
            }
            long userid = ValidUserUtility.ValidUser();
            ViewBag.AllNotifications = notificationService.GetNotificationService(userid);
            if (type!=null)
            {
                Notification notification = notificationService.RemoveNotificationService(long.Parse(id));
                
            }
            return View();
        }
	}
}