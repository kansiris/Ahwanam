using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaaAahwanam.Repository.db;
using MaaAahwanam.Models;

namespace MaaAahwanam.Service
{
    public class EventsService
    {
        EventInformationRepository eventInformationRepository = new EventInformationRepository();

        public int EventInformationCount()
        {
            int l1 = eventInformationRepository.EventInformationList().Count();
            return l1;
        }        
        public EventInformation SaveEventinformation(EventInformation eventInformation)
        {
            eventInformation=eventInformationRepository.PostEventDetails(eventInformation);
            return eventInformation;
        }
        public void updateeventid(long CartId, long OrderID)
        {
            eventInformationRepository.updateeventid(CartId, OrderID);
        }
        //public void updateeventodid(long Vid, long sid, long OrderDetailsID)
        //{
        //    eventInformationRepository.updateeventoids(Vid,sid, OrderDetailsID);
        //}
        public void updateeventodid(long cartid, long OrderDetailsID)
        {
            eventInformationRepository.updateeventoids(cartid, OrderDetailsID);
        }
    }
}