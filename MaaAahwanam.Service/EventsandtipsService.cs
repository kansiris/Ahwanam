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
    public class EventsandtipsService
    {
        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        EventsandTipRepository eventsandTipRepository = new EventsandTipRepository();
        public List<geteventsandtipsimages_Result> EventsandTipsList(int id)
        {
            return eventsandTipRepository.EventsandTipList(id);
        }

        public EventsandTip AddEventandTip(EventsandTip eventAndTip)
        {
            DateTime updateddate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
            eventAndTip.UpdatedDate = updateddate;
            eventAndTip.Status = "Active";
            eventsandTipRepository.AddEventsAndTip(eventAndTip);
            return eventAndTip;
        }

        public long EventIDCount()
        {
            return eventsandTipRepository.EventIdCount();
        }

        public EventsandTip GetEventandTip(long id)
        {
            return eventsandTipRepository.GetEventsAndTip(id);
        }

        public EventsandTip UpdateEventandTip(EventsandTip eventAndTip,long id)
        {
            DateTime updateddate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
            eventAndTip.UpdatedDate = updateddate;
            eventAndTip.Status = "Active";
            eventsandTipRepository.UpdateEventsAndTip(eventAndTip,id);
            return eventAndTip;
        }
        public List<geteventsandtipsimages_Result> EventsandTipsListUser(string type,int id)
        {
            return eventsandTipRepository.EventsandTipList(id).Where(o=>o.Type== type).ToList();
        }
    }
}
