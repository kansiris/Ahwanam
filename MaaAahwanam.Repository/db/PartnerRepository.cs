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
        public PartnerFile addPartnerfile(PartnerFile partnerFile)
        {
            _dbContext.PartnerFile.Add(partnerFile);
            _dbContext.SaveChanges();
            return partnerFile;
        }
        public Partner getPartner(string email)
        {
            Partner partner = new Partner();
            if (email != null)
                partner = _dbContext.Partner.Where(p => p.emailid == email).FirstOrDefault();
            return partner;
           
        }
        public Partner UpdatePartner(Partner partner,long partid)
        {
            var partner1 = _dbContext.Partner.Where(m => m.PartnerID == partid).FirstOrDefault();
            partner.PartnerID = partner1.PartnerID;
            _dbContext.Entry(partner1).CurrentValues.SetValues(partner);
            _dbContext.SaveChanges();
            return partner1;
        }
        public PartnerContact UpdatePartnercontact(PartnerContact Partnercontact)
        {
            _dbContext.PartnerContact.Add(Partnercontact);
            _dbContext.SaveChanges();
            return Partnercontact;
        }

        public List<PartnerFile> GetFiles(string vid,string partid)
        {
            return _dbContext.PartnerFile.Where(p => p.VendorID == vid && p.PartnerID==partid).ToList();

        }

        public List<Partner> GetPartners(long vid)
        {
            return _dbContext.Partner.Where(p => p.VendorId == vid).ToList();
           
        }
        public List<PartnerPackage> getPartnerPackage(long vid)
        {
            return _dbContext.PartnerPackage.Where(p => p.VendorId == vid).ToList();

        }
        public List<PartnerContact> getPartnercontact(string vid)
        {
            return _dbContext.PartnerContact.Where(p => p.VendorID == vid).ToList();

        }
        public List<PartnerPackage> getallPartnerPackage()
        {
            return _dbContext.PartnerPackage.ToList();

        }
        public List<Partner> GetallPartners()
        {
            return _dbContext.Partner.ToList();

        }
        public PartnerPackage addPartnerPackage(PartnerPackage partnerPackage)
        {
            _dbContext.PartnerPackage.Add(partnerPackage);
            _dbContext.SaveChanges();
            return partnerPackage;
        }
        public PartnerPackage updatepartnerpackage(PartnerPackage partnerPackage, long partid, long packageid)
        {
            //var partnerPackage1 = _dbContext.PartnerPackage.Where(m => m.PartnerID == partid && m.packageid == packageid.ToString()).FirstOrDefault();
            //partnerPackage.ID = partnerPackage1.ID;
            //_dbContext.Entry(partnerPackage1).CurrentValues.SetValues(partnerPackage);
            //_dbContext.SaveChanges();
            // Query the database for the row to be updated.
            var query =
                from ord in _dbContext.PartnerPackage
                where ord.PartnerID == partid && ord.packageid == packageid.ToString()
                select ord;
            // Query the database for the row to be updated.

            // Execute the query, and change the column values
            // you want to change.
            foreach (PartnerPackage ord in query)
            { 
                ord.Achoicedays = partnerPackage.Achoicedays;
                ord.Anormaldays = partnerPackage.Anormaldays;
                ord.Aholidays = partnerPackage.Aholidays;
                ord.Apeakdays = partnerPackage.Apeakdays;
                ord.Status = partnerPackage.Status;
                ord.UpdatedDate = partnerPackage.UpdatedDate;
                // Insert any additional changes to column values.
            }
            // Query the database for the row to be updated.

            try
            {
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {

            }
            return partnerPackage;
        }
    }
}
