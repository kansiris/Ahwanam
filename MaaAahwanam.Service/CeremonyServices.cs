using MaaAahwanam.Models;
using MaaAahwanam.Repository.db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaaAahwanam.Service
{
    public class CeremonyServices
    {
        CeremonyRepository ceremonyrepo = new CeremonyRepository();

         public List<Ceremony> Getall()
        {
            return ceremonyrepo.Getall();
        }
        public List<CeremonyCategory> getceremonydetails(long id)
        {
            return ceremonyrepo.getceremonycategory(id);
        }
   }
}
