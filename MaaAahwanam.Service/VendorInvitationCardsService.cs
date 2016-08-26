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
        UserDetailsRepository userDetailsRepository = new UserDetailsRepository();
        UserLogin userLogin = new UserLogin();
        UserDetail userDetail = new UserDetail();
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
            userDetail.UserLoginId = userLogin.UserLoginId;
            userDetail.FirstName = vendorMaster.BusinessName;
            userDetail.UserPhone = vendorMaster.ContactNumber;
            userDetail.Url = vendorMaster.Url;
            userDetail.Address = vendorMaster.Address;
            userDetail.City = vendorMaster.City;
            userDetail.State = vendorMaster.State;
            userDetail.ZipCode = vendorMaster.ZipCode;
            userDetail.Status = "Active";
            userDetail.UpdatedBy = ValidUserUtility.ValidUser();
            userDetail.UpdatedDate = DateTime.Now;
            userDetail.AlternativeEmailID = vendorMaster.EmailId;
            userDetail.Landmark = vendorMaster.Landmark;
            userDetail = userDetailsRepository.AddUserDetails(userDetail);
            if (vendorMaster.Id != 0 && vendorInvitationCard.Id != 0 && userLogin.UserLoginId != 0 && userDetail.UserDetailId != 0)
            {
                return vendorInvitationCard;
            }
            else
            {
                vendorInvitationCard.Id = 0;
                return vendorInvitationCard;
            }
        }
        public VendorsInvitationCard GetVendorInvitationCard(long id,long vid)
        {
            return vendorInvitationCardsRepository.GetVendorsInvitationCard(id,vid);
        }

        public VendorsInvitationCard UpdatesInvitationCard(VendorsInvitationCard vendorsInvitationCard, Vendormaster vendorMaster, long masterid,long vid)
        {
            vendorsInvitationCard.Status = "Active";
            vendorsInvitationCard.UpdatedDate = DateTime.Now;
            vendorMaster.Status = "Active";
            vendorMaster.UpdatedDate = DateTime.Now;
            vendorMaster.ServicType = "InvitationCard";
            vendorMaster = vendorMasterRepository.UpdateVendorMaster(vendorMaster, masterid);
            vendorsInvitationCard = vendorInvitationCardsRepository.UpdatesInvitationCard(vendorsInvitationCard, masterid,vid);
            return vendorsInvitationCard;
        }

        public VendorsInvitationCard AddNewInvitationCard(VendorsInvitationCard vendorsInvitationCard, Vendormaster vendorMaster)
        {
            vendorsInvitationCard.Status = "Active";
            vendorsInvitationCard.UpdatedDate = DateTime.Now;
            vendorsInvitationCard.VendorMasterId = vendorMaster.Id;
            vendorsInvitationCard = vendorInvitationCardsRepository.AddInvitationCards(vendorsInvitationCard);
            return vendorsInvitationCard;
        }
    }
}
