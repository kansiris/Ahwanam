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
      
    }
}
