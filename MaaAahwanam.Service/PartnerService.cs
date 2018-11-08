using MaaAahwanam.Models;
using MaaAahwanam.Repository.db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaaAahwanam.Service
{
   public class PartnerService
    {
        PartnerRepository partnerrepo = new PartnerRepository();
        public Partner AddPartner(Partner partner)
        {
            partner = partnerrepo.AddPartner(partner);
            return partner;
        }
        public PartnerFile addPartnerfile(PartnerFile partnerFile)
        {
            partnerFile = partnerrepo.addPartnerfile(partnerFile);
            return partnerFile;
        }
        public Partner getPartner(string email)
        {
            Partner partner = new Partner();

            partner = partnerrepo.getPartner(email);
            return partner;
        }
        public Partner UpdatePartner(Partner partner,string partid)
        {
            return partnerrepo.UpdatePartner(partner, long.Parse(partid));
        }
        public PartnerContact UpdatePartnercontact(PartnerContact Partnercontact)
        {
            return partnerrepo.UpdatePartnercontact(Partnercontact);
        }
        public List<PartnerFile> GetFiles(string vid,string partid)
        {
            return partnerrepo.GetFiles(vid,partid);
        }
        public List<Partner> GetPartners(string vid)
        {
            return partnerrepo.GetPartners(long.Parse(vid));
        }

        public List<PartnerPackage> getPartnerPackage(string vid)
        {
            return partnerrepo.getPartnerPackage(long.Parse(vid));
        }
        public List<PartnerPackage> getallPartnerPackage()
        {
            return partnerrepo.getallPartnerPackage();
        }
        public List<Partner> GetallPartners()
        {
            return partnerrepo.GetallPartners();
        }

        public PartnerPackage addPartnerPackage(PartnerPackage partnerPackage)
        {
            partnerPackage = partnerrepo.addPartnerPackage(partnerPackage);
            return partnerPackage;
        }
        public PartnerPackage updatepartnerpackage(PartnerPackage partnerPackage ,long partid, long packageid)
        {
            partnerPackage = partnerrepo.updatepartnerpackage(partnerPackage, partid,packageid );
            return partnerPackage;
        }
    }
}
