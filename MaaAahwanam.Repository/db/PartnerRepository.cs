using MaaAahwanam.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaaAahwanam.Repository.db
{
   public class PartnerRepository
    {
        readonly ApiContext _dbContext = new ApiContext();

        public Partner AddPartner(Partner partner)
        {
            _dbContext.Partner.Add(partner);
            _dbContext.SaveChanges();
            return partner;
        }
    }
}
