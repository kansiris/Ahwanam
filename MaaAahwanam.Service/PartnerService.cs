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
        public List<Partner> GetPartners(string vid)
        {
            return partnerrepo.GetPartners(long.Parse(vid));
        }

        public List<PartnerPackage> getPartnerPackage(string vid)
        {
            return partnerrepo.getPartnerPackage(long.Parse(vid));
        }

        public PartnerPackage addPartnerPackage(PartnerPackage partnerPackage)
        {
            partnerPackage = partnerrepo.addPartnerPackage(partnerPackage);
            return partnerPackage;
        }
        public PartnerPackage updatepartnerpackage(PartnerPackage partnerPackage ,long partid, string date, long packageid)
        {
            partnerPackage = partnerrepo.updatepartnerpackage(partnerPackage, partid, date, packageid);
            return partnerPackage;
        }
    }
}
