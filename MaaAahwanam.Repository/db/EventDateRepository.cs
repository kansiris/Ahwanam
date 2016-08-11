using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaaAahwanam.Models;

namespace MaaAahwanam.Repository.db
{
    public class EventDateRepository
    {
        readonly ApiContext _dbContext = new ApiContext();
        public EventDate PostEventDatesDetails(EventDate eventDate)
        {
            _dbContext.EventDate.Add(eventDate);
            _dbContext.SaveChanges();
            return eventDate;
        }
    }
}
