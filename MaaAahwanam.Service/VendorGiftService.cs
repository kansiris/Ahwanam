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
   public class VendorGiftService
    {
        RandomPassword randomPassword = new RandomPassword();
        UserLoginRepository userLoginRepository = new UserLoginRepository();
        UserDetailsRepository userDetailsRepository = new UserDetailsRepository();
        UserLogin userLogin = new UserLogin();
        UserDetail userDetail = new UserDetail();
        VendormasterRepository vendorMasterRepository = new VendormasterRepository();
        VendorGiftsRepository vendorGiftsRepository = new VendorGiftsRepository();
        public VendorsGift AddGift(VendorsGift vendorGift, Vendormaster vendorMaster)
       {
            string updateddate = DateTime.UtcNow.ToShortDateString();
            vendorGift.Status = "Active";
           vendorGift.UpdatedDate  = Convert.ToDateTime(updateddate);
           vendorMaster.Status = "Active";
           vendorMaster.UpdatedDate = Convert.ToDateTime(updateddate);
           vendorMaster.ServicType = "Gifts";
           vendorMaster = vendorMasterRepository.AddVendorMaster(vendorMaster);
           vendorGift.VendorMasterId = vendorMaster.Id;
           vendorGift = vendorGiftsRepository.AddGifts(vendorGift);
            userLogin.UserName = vendorMaster.EmailId;
            userLogin.Password = randomPassword.GenerateString();
            userLogin.UserType = "Vendor";
            userLogin.UpdatedBy = 2;
            userLogin.RegDate = Convert.ToDateTime(updateddate);
            userLogin.UpdatedDate = Convert.ToDateTime(updateddate);
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
            userDetail.UpdatedDate = Convert.ToDateTime(updateddate);
            userDetail.AlternativeEmailID = vendorMaster.EmailId;
            userDetail.Landmark = vendorMaster.Landmark;
            userDetail = userDetailsRepository.AddUserDetails(userDetail);
            if (vendorMaster.Id != 0 && vendorGift.Id != 0 && userLogin.UserLoginId != 0 && userDetail.UserDetailId != 0)
            {
                return vendorGift;
            }
            else
            {
                vendorGift.Id = 0;
                return vendorGift;
            }
        }
        public VendorsGift GetVendorGift(long id,long vid)
        {
            return vendorGiftsRepository.GetVendorsGift(id,vid);
        }

        public VendorsGift UpdatesGift(VendorsGift vendorsGift, Vendormaster vendorMaster, long masterid, long vid)
        {
            string updateddate = DateTime.UtcNow.ToShortDateString();
            vendorsGift.Status = "Active";
            vendorsGift.UpdatedDate = Convert.ToDateTime(updateddate);
            vendorMaster.Status = "Active";
            vendorMaster.UpdatedDate = Convert.ToDateTime(updateddate);
            vendorMaster.ServicType = "Gifts";
            vendorMaster = vendorMasterRepository.UpdateVendorMaster(vendorMaster, masterid);
            vendorsGift = vendorGiftsRepository.UpdatesGift(vendorsGift, masterid,vid);
            return vendorsGift;
        }

        public VendorsGift activationGift(VendorsGift vendorsGift, Vendormaster vendorMaster, long masterid, long vid)
        {
            string updateddate = DateTime.UtcNow.ToShortDateString();
            //vendorsGift.Status = "Active";
            vendorsGift.UpdatedDate = Convert.ToDateTime(updateddate);
            //vendorMaster.Status = "Active";
            vendorMaster.UpdatedDate = Convert.ToDateTime(updateddate);
            vendorMaster.ServicType = "Gifts";
            vendorMaster = vendorMasterRepository.UpdateVendorMaster(vendorMaster, masterid);
            vendorsGift = vendorGiftsRepository.UpdatesGift(vendorsGift, masterid, vid);
            return vendorsGift;
        }

        public VendorsGift AddNewGift(VendorsGift vendorsGift, Vendormaster vendorMaster)
        {
            string updateddate = DateTime.UtcNow.ToShortDateString();
            vendorsGift.Status = "Active";
            vendorsGift.UpdatedDate = Convert.ToDateTime(updateddate);
            vendorsGift.VendorMasterId = vendorMaster.Id;
            vendorsGift = vendorGiftsRepository.AddGifts(vendorsGift);
            return vendorsGift;
        }

    }
}
