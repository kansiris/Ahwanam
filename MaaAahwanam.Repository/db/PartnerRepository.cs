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
        public Partner getPartner(string email)
        {
            Partner partner = new Partner();
            if (email != null)
                partner = _dbContext.Partner.SingleOrDefault(p => p.emailid == email);
            return partner;
           
        }
        public Partner UpdatePartner(Partner partner,long partid)
        {
            var partner1 = _dbContext.Partner.Where(m => m.PartnerID == partid).FirstOrDefault();

            //var partner1 = _dbContext.Partner.SingleOrDefault(v => v.PartnerID == long.Parse(partid));
            //partner.PartnerID = partner1.PartnerID;
            //partner.VendorId = partner1.VendorId;
            //partner.RegisteredDate = partner1.RegisteredDate;
            _dbContext.Entry(partner1).CurrentValues.SetValues(partner);
            _dbContext.SaveChanges();
            return partner1;
        }
    }
}
