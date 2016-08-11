using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaaAahwanam.Repository.db;
using MaaAahwanam.Models;

namespace MaaAahwanam.Service
{
    public class EventDatesServices
    {
        public string SaveEventDates(EventDate EventDate)
        {
            string message = "";
            EventDateRepository eventDateRepository = new EventDateRepository();
            EventDate = eventDateRepository.PostEventDatesDetails(EventDate);
            if (EventDate != null)
            {
                if (EventDate.EventId != 0)
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
