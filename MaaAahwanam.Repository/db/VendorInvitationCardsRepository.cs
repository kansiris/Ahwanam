using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaaAahwanam.Models;

namespace MaaAahwanam.Repository.db
{
   public class VendorInvitationCardsRepository
    {
        readonly ApiContext _dbContext = new ApiContext();
        public List<dynamic> VendorsInvitationCardsList()
        {
            return _dbContext.VendorsInvitationCard.Join(_dbContext.Vendormaster, i => i.VendorMasterId, p => p.Id, (i, p) => new { p = p, i = i }).ToList<dynamic>();

        }

        public VendorsInvitationCard AddInvitationCards(VendorsInvitationCard vendorsInvitationCards)
        {
            _dbContext.VendorsInvitationCard.Add(vendorsInvitationCards);
            _dbContext.SaveChanges();
            return vendorsInvitationCards;
        }
        public VendorsInvitationCard GetVendorsInvitationCard(long id, long vid)
        {
            return _dbContext.VendorsInvitationCard.Where(m => m.VendorMasterId == id && m.Id == vid).FirstOrDefault();
        }

        public VendorsInvitationCard UpdatesInvitationCard(VendorsInvitationCard vendorsInvitationCard, long id, long vid)
        {
            var GetVendor = _dbContext.VendorsInvitationCard.SingleOrDefault(m => m.VendorMasterId == id && m.Id == vid);
            vendorsInvitationCard.Id = GetVendor.Id;
            _dbContext.Entry(GetVendor).CurrentValues.SetValues(vendorsInvitationCard);
            _dbContext.SaveChanges();
            return vendorsInvitationCard;
        }
    }
}
