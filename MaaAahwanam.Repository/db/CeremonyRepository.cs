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
        public List<Ceremony> Getall()
        {
            return _dbContext.Ceremony.ToList();
        }

        public CeremonyCategory getceremonycategory(long id)
        {
            return _dbContext.CeremonyCategory.Where(c => c.CeremonyId == id).FirstOrDefault();
        }
    }
}
