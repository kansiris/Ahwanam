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
        public EventDate PostEventDatesDetails(EventDate eventDate)
        {
            _dbContext.EventDate.Add(eventDate);
            _dbContext.SaveChanges();
            return eventDate;
        }
    }
}
