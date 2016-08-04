﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaaAahwanam.Repository.db
{
    public class DashboardRepository
    {
        readonly ApiContext _dbContext = new ApiContext();

        public int VendorsCount()
        {
            return _dbContext.Vendormaster.ToList().Count();
        }

        public int CommentsCount()
        {
            return _dbContext.Comment.ToList().Count();
        }

        public int TicketsCount()
        {
            return _dbContext.IssueTicket.ToList().Count();
        }

        public int OrdersCount()
        {
            return _dbContext.Order.ToList().Count();
        }
        
    }
}
