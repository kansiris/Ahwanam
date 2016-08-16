using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaaAahwanam.Models;

namespace MaaAahwanam.Repository.db
{
    public class OrderdetailsRepository
    {
        readonly ApiContext _dbContext = new ApiContext();
        public OrderDetail SaveOrderDetail(OrderDetail orderDetail)
        {
            _dbContext.OrderDetail.Add(orderDetail);
            _dbContext.SaveChanges();
            return orderDetail;
        }
    }
}
