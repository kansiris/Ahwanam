using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaaAahwanam.Models;
using MaaAahwanam.Repository;

namespace MaaAahwanam.Repository.db
{
    public class EventsandTipRepository
    {
        readonly ApiContext _dbContext = new ApiContext();
        MaaAahwanamEntities maaAahwanamEntities = new MaaAahwanamEntities();
        public List<geteventsandtipsimages_Result> EventsandTipList()
        {
            return maaAahwanamEntities.geteventsandtipsimages().ToList();
        }

        public EventsandTip AddEventsAndTip(EventsandTip eventAndTip)
        {
            _dbContext.EventsandTip.Add(eventAndTip);
            _dbContext.SaveChanges();
            return eventAndTip;
        }

        public long EventIdCount()
        {
            var EventIdCount = _dbContext.EventsandTip.DefaultIfEmpty().Max(r => r == null ? 0 : r.EventId);
            if (EventIdCount == 0)
            {
                return EventIdCount + 1;
            }
            return EventIdCount + 1;
        }

        public EventsandTip GetEventsAndTip(long id)
        {
            return _dbContext.EventsandTip.Where(m => m.EventId == id).FirstOrDefault();
        }

        public EventsandTip UpdateEventsAndTip(EventsandTip eventAndTip,long id)
        {
            var GetRecord = _dbContext.EventsandTip.Where(m => m.EventId == id).FirstOrDefault();
            eventAndTip.EventId = GetRecord.EventId;
            eventAndTip.UpdatedBy = GetRecord.UpdatedBy;
            eventAndTip.Image = GetRecord.Image;
            _dbContext.Entry(GetRecord).CurrentValues.SetValues(eventAndTip);
            _dbContext.SaveChanges();
            return eventAndTip;
        }

       
    }
}
