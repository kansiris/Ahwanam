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
    }
}
