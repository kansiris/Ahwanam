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
        UserLogin userLogin = new UserLogin();
        VendormasterRepository vendorMasterRepository = new VendormasterRepository();
        VendorGiftsRepository vendorGiftsRepository = new VendorGiftsRepository();
        public VendorsGift AddGift(VendorsGift vendorGift, Vendormaster vendorMaster)
       {
           vendorGift.Status = "Active";
           vendorGift.UpdatedDate  = DateTime.Now;
           vendorMaster.Status = "Active";
           vendorMaster.UpdatedDate = DateTime.Now;
           vendorMaster.ServicType = "Gifts";
           vendorMaster = vendorMasterRepository.AddVendorMaster(vendorMaster);
           vendorGift.VendorMasterId = vendorMaster.Id;
           vendorGift = vendorGiftsRepository.AddGifts(vendorGift);
            userLogin.UserName = vendorMaster.EmailId;
            userLogin.Password = randomPassword.GenerateString();
            userLogin.UserType = "Vendor";
            userLogin.UpdatedBy = 2;
            userLogin.RegDate = DateTime.Now;
            userLogin.UpdatedDate = DateTime.Now;
            userLogin.Status = "Active";
            userLogin = userLoginRepository.AddVendorUserLogin(userLogin);
            if (vendorMaster.Id != 0 && vendorGift.Id != 0 && userLogin.UserLoginId != 0)
            {
                return vendorGift;
            }
            else
            {
                vendorGift.Id = 0;
                return vendorGift;
            }
        }
        public VendorsGift GetVendorGift(long id)
        {
            return vendorGiftsRepository.GetVendorsGift(id);
        }

        public VendorsGift UpdatesGift(VendorsGift vendorsGift, Vendormaster vendorMaster, long masterid)
        {
            vendorsGift.Status = "Active";
            vendorsGift.UpdatedDate = DateTime.Now;
            vendorMaster.Status = "Active";
            vendorMaster.UpdatedDate = DateTime.Now;
            vendorMaster.ServicType = "Gifts";
            vendorMaster = vendorMasterRepository.UpdateVendorMaster(vendorMaster, masterid);
            vendorsGift = vendorGiftsRepository.UpdatesGift(vendorsGift, masterid);
            return vendorsGift;
        }
    }
}
