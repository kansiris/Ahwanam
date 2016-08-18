using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaaAahwanam.Models;

namespace MaaAahwanam.Repository.db
{
    public class NotificationRepository
    {
        readonly ApiContext _dbContext = new ApiContext();
        public List<Notification> GetNotificationList(long id)
        {
            return _dbContext.Notification.Where(m=>m.NotificationTo == id && m.type== "User" && m.Status == "Active").ToList();
        }

        public Notification RemoveNotification(long id)
        {
            Notification notification = new Notification();
            var getnotification = _dbContext.Notification.Where(m => m.id == id).FirstOrDefault();
            notification.id = id;
            notification.Status = "Suspended";
            notification.type = getnotification.type;
            notification.description = getnotification.description;
            notification.Subject = getnotification.Subject;
            notification.EmailID = getnotification.EmailID;
            notification.NotificationTo = getnotification.NotificationTo;
            notification.DateandTime = getnotification.DateandTime;
            _dbContext.Entry(getnotification).CurrentValues.SetValues(notification);
            _dbContext.SaveChanges();
            return notification;
        }
    }
}
