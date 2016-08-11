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
        public int EventInformationCount()
        {
            EventInformationRepository eventInformationRepository = new EventInformationRepository();
            int l1 = eventInformationRepository.EventInformationList().Count();
            return l1;
        }        
        public string SaveEventinformation(EventInformation eventInformation)
        {
            string message = "";
            EventInformationRepository eventInformationRepository = new EventInformationRepository();
            eventInformation=eventInformationRepository.PostEventDetails(eventInformation);
            if (eventInformation  != null)
            {
                if (eventInformation.EventId != null)
                    message = "Success";
                else
                    message = "Failed";
            }
            else
            {
                message = "Failed";
            }
            return message;
        }
    }
}