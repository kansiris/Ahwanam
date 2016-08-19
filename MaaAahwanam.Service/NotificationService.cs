using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaaAahwanam.Repository.db;
using MaaAahwanam.Models;
using MaaAahwanam.Repository;

namespace MaaAahwanam.Service
{
    public class NotificationService
    {
        NotificationRepository notificationRepository = new NotificationRepository();
        public List<Notification> GetNotificationService(long id)
        {
            return notificationRepository.GetNotificationList(id);
        }

        public Notification RemoveNotificationService(long id)
        {
            return notificationRepository.RemoveNotification(id);
        }
    }
}
