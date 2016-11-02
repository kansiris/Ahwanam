using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaaAahwanam.Models;

namespace MaaAahwanam.Repository.db
{
    public class EventInformationRepository
    {
        readonly ApiContext _dbContext = new ApiContext();
        public List<EventInformation> EventInformationList()
        {
            return _dbContext.EventInformation.ToList();
        }

        public EventInformation PostEventDetails(EventInformation eventInformation)
        {
            _dbContext.EventInformation.Add(eventInformation);
            _dbContext.SaveChanges();
            return eventInformation;
        }
        public void updateeventid(long CartId, long OrderID)
        {
            var list = _dbContext.EventInformation.Where(m => m.CartId == CartId).FirstOrDefault();
            list.OrderId = OrderID;
            _dbContext.SaveChanges();
        }
        //public void updateeventoids(long Vid,long sid,long OrderDetailsID)
        //{
        //    var list = _dbContext.EventInformation.Where(s=>s.vendorid==Vid && s.subid == sid).FirstOrDefault();
        //    list.OrderDetailsid = OrderDetailsID;
        //    _dbContext.Entry(list).CurrentValues.SetValues(list);
        //    _dbContext.SaveChanges();
        //}
        public void updateeventoids(long cartid, long OrderDetailsID)
        {
            var list = _dbContext.EventInformation.Where(s => s.CartId == cartid).FirstOrDefault();
            list.OrderDetailsid = OrderDetailsID;
            _dbContext.Entry(list).CurrentValues.SetValues(list);
            _dbContext.SaveChanges();
        }
    }
}
