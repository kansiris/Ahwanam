using MaaAahwanam.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace MaaAahwanam.Repository.db
{
   public class CeremonyRepository
    {
        readonly ApiContext _dbContext = new ApiContext();
        MaaAahwanamEntities maaAahwanamEntities = new MaaAahwanamEntities();
        public List<Ceremony> Getall()
        {
            return _dbContext.Ceremony.ToList();
        }
        public List<Ceremony> Getalleventtype(int type)
        {
            return _dbContext.Ceremony.Where(c => c.type == type).ToList();
        }
        public Ceremony Getceremonydetails(string pagename)
        {
            return _dbContext.Ceremony.Where(c => c.page_name == pagename).FirstOrDefault();
        }

        public List<CeremonyCategory> getceremonycategory(long id)
        {
            return _dbContext.CeremonyCategory.Where(c => c.CeremonyId == id).ToList();
        }

        public List<Ceremonydetails_Result> GetDetails(string ceremony)
        {
            return maaAahwanamEntities.Ceremonydetails(ceremony).ToList();
        }

    }
}
