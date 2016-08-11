using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaaAahwanam.Repository.db;
using MaaAahwanam.Models;

namespace MaaAahwanam.Service
{
    public class AdminDashboardService
    {
        AdminDashboardRepository AdmindashboardRepository = new AdminDashboardRepository();

        public int VendorsCountService()
        {
            return AdmindashboardRepository.VendorsCount();
        }

        public int CommentsCountService()
        {
            return AdmindashboardRepository.CommentsCount();
        }

        public int TicketsCountService()
        {
            return AdmindashboardRepository.TicketsCount();
        }

        public int OrdersCountService()
        {
            return AdmindashboardRepository.OrdersCount();
        }

        public UserDetail AdminNameService(long id)
        {
            return AdmindashboardRepository.AdminName(id);
        }
    }
}
