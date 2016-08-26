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
        EventsandTipRepository eventsandTipRepository = new EventsandTipRepository();
        public List<geteventsandtipsimages_Result> EventsandTipsList()
        {
            return eventsandTipRepository.EventsandTipList();
            
        }

        public EventsandTip AddEventandTip(EventsandTip eventAndTip)
        {
            eventAndTip.UpdatedDate = DateTime.Now;
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
            eventAndTip.UpdatedDate = DateTime.Now;
            eventAndTip.Status = "Active";
            eventsandTipRepository.UpdateEventsAndTip(eventAndTip,id);
            return eventAndTip;
        }
    }
}
