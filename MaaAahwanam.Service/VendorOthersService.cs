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
   public class VendorOthersService
    {
        RandomPassword randomPassword = new RandomPassword();
        UserLoginRepository userLoginRepository = new UserLoginRepository();
        UserDetailsRepository userDetailsRepository = new UserDetailsRepository();
        UserLogin userLogin = new UserLogin();
        UserDetail userDetail = new UserDetail();
        VendormasterRepository vendorMasterRepository = new VendormasterRepository();
        VendorOthersRepository vendorOthersRepository = new VendorOthersRepository();
        public VendorsOther AddOther(VendorsOther vendorOther, Vendormaster vendorMaster)
       {
           vendorOther.Status = "Active";
           vendorOther.UpdatedDate =  DateTime.Now;
           vendorMaster.Status = "Active";
           vendorMaster.UpdatedDate = DateTime.Now;
           vendorMaster.ServicType = "Other";
           vendorMaster = vendorMasterRepository.AddVendorMaster(vendorMaster);
           vendorOther.VendorMasterId = vendorMaster.Id;
           vendorOther = vendorOthersRepository.AddOthers(vendorOther);
            userLogin.UserName = vendorMaster.EmailId;
            userLogin.Password = randomPassword.GenerateString();
            userLogin.UserType = "Vendor";
            userLogin.Status = "Active";
            userLogin.UpdatedBy = 2;
            userLogin.RegDate = DateTime.Now;
            userLogin.UpdatedDate = DateTime.Now;
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
            if (vendorMaster.Id != 0 && vendorOther.Id != 0 && userLogin.UserLoginId != 0 && userDetail.UserDetailId != 0)
            {
                return vendorOther;
            }
            else
            {
                vendorOther.Id = 0;
                return vendorOther;
            }
        }

        public VendorsOther GetVendorOther(long id, long vid)
        {
            return vendorOthersRepository.GetVendorOthers(id, vid);
        }

        public VendorsOther UpdateOther(VendorsOther vendorOther, Vendormaster vendorMaster, long masterid, long vid)
        {
            vendorOther.Status = "Active";
            vendorOther.UpdatedDate = DateTime.Now;
            vendorMaster.Status = "Active";
            vendorMaster.UpdatedDate = DateTime.Now;
            vendorMaster.ServicType = "Other";
            vendorMaster = vendorMasterRepository.UpdateVendorMaster(vendorMaster, masterid);
            vendorOther = vendorOthersRepository.UpdateOthers(vendorOther, masterid,vid);
            return vendorOther;
        }
        public VendorsOther AddNewOther(VendorsOther vendorsOther, Vendormaster vendorMaster)
        {
            vendorsOther.Status = "Active";
            vendorsOther.UpdatedDate = DateTime.Now;
            vendorsOther.VendorMasterId = vendorMaster.Id;
            vendorsOther = vendorOthersRepository.AddOthers(vendorsOther);
            return vendorsOther;
        }
    }
}
