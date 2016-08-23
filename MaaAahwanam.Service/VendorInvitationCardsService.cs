using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaaAahwanam.Repository.db;
using MaaAahwanam.Models;
using MaaAahwanam.Utility;

namespace MaaAahwanam.Service
{
   public class VendorInvitationCardsService
    {
        RandomPassword randomPassword = new RandomPassword();
        UserLoginRepository userLoginRepository = new UserLoginRepository();
        UserLogin userLogin = new UserLogin();
        VendormasterRepository vendorMasterRepository = new VendormasterRepository();
        VendorInvitationCardsRepository vendorInvitationCardsRepository = new VendorInvitationCardsRepository();
        public VendorsInvitationCard AddInvitationCard(VendorsInvitationCard vendorInvitationCard, Vendormaster vendorMaster)
       {
           vendorInvitationCard.Status = "Active";
           vendorInvitationCard.UpdatedDate =  DateTime.Now;
           vendorMaster.Status = "Active";
           vendorMaster.UpdatedDate = DateTime.Now;
           vendorMaster.ServicType = "InvitationCard";
           vendorMaster = vendorMasterRepository.AddVendorMaster(vendorMaster);
           vendorInvitationCard.VendorMasterId = vendorMaster.Id;
           vendorInvitationCard = vendorInvitationCardsRepository.AddInvitationCards(vendorInvitationCard);
            userLogin.UserName = vendorMaster.EmailId;
            userLogin.Password = randomPassword.GenerateString();
            userLogin.UserType = "Vendor";
            userLogin.UpdatedBy = 2;
            userLogin.RegDate = DateTime.Now;
            userLogin.UpdatedDate = DateTime.Now;
            userLogin.Status = "Active";
            userLogin = userLoginRepository.AddVendorUserLogin(userLogin);
            if (vendorMaster.Id != 0 && vendorInvitationCard.Id != 0 && userLogin.UserLoginId != 0)
            {
                return vendorInvitationCard;
            }
            else
            {
                vendorInvitationCard.Id = 0;
                return vendorInvitationCard;
            }
        }
        public VendorsInvitationCard GetVendorInvitationCard(long id)
        {
            return vendorInvitationCardsRepository.GetVendorsInvitationCard(id);
        }

        public VendorsInvitationCard UpdatesInvitationCard(VendorsInvitationCard vendorsInvitationCard, Vendormaster vendorMaster, long masterid)
        {
            vendorsInvitationCard.Status = "Active";
            vendorsInvitationCard.UpdatedDate = DateTime.Now;
            vendorMaster.Status = "Active";
            vendorMaster.UpdatedDate = DateTime.Now;
            vendorMaster.ServicType = "InvitationCard";
            vendorMaster = vendorMasterRepository.UpdateVendorMaster(vendorMaster, masterid);
            vendorsInvitationCard = vendorInvitationCardsRepository.UpdatesInvitationCard(vendorsInvitationCard, masterid);
            return vendorsInvitationCard;
        }
    }
}
