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
            string updateddate = DateTime.UtcNow.ToShortDateString();
            vendorOther.Status = "Active";
           vendorOther.UpdatedDate =  Convert.ToDateTime(updateddate);
           vendorMaster.Status = "Active";
           vendorMaster.UpdatedDate = Convert.ToDateTime(updateddate);
           vendorMaster.ServicType = "Other";
           vendorMaster = vendorMasterRepository.AddVendorMaster(vendorMaster);
           vendorOther.VendorMasterId = vendorMaster.Id;
           vendorOther = vendorOthersRepository.AddOthers(vendorOther);
            userLogin.UserName = vendorMaster.EmailId;
            userLogin.Password = randomPassword.GenerateString();
            userLogin.UserType = "Vendor";
            userLogin.Status = "Active";
            userLogin.UpdatedBy = 2;
            userLogin.RegDate = Convert.ToDateTime(updateddate);
            userLogin.UpdatedDate = Convert.ToDateTime(updateddate);
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
            string updateddate = DateTime.UtcNow.ToShortDateString();
            vendorOther.Status = "Active";
            vendorOther.UpdatedDate = Convert.ToDateTime(updateddate);
            vendorMaster.Status = "Active";
            vendorMaster.UpdatedDate = Convert.ToDateTime(updateddate);
            vendorMaster.ServicType = "Other";
            vendorMaster = vendorMasterRepository.UpdateVendorMaster(vendorMaster, masterid);
            vendorOther = vendorOthersRepository.UpdateOthers(vendorOther, masterid,vid);
            return vendorOther;
        }

        public VendorsOther activationOther(VendorsOther vendorOther, Vendormaster vendorMaster, long masterid, long vid)
        {
            string updateddate = DateTime.UtcNow.ToShortDateString();
            //vendorOther.Status = "Active";
            vendorOther.UpdatedDate = Convert.ToDateTime(updateddate);
            //vendorMaster.Status = "Active";
            vendorMaster.UpdatedDate = Convert.ToDateTime(updateddate);
            vendorMaster.ServicType = "Other";
            vendorMaster = vendorMasterRepository.UpdateVendorMaster(vendorMaster, masterid);
            vendorOther = vendorOthersRepository.UpdateOthers(vendorOther, masterid, vid);
            return vendorOther;
        }
        public VendorsOther AddNewOther(VendorsOther vendorsOther, Vendormaster vendorMaster)
        {
            string updateddate = DateTime.UtcNow.ToShortDateString();
            vendorsOther.Status = "Active";
            vendorsOther.UpdatedDate = Convert.ToDateTime(updateddate);
            vendorsOther.VendorMasterId = vendorMaster.Id;
            vendorsOther = vendorOthersRepository.AddOthers(vendorsOther);
            return vendorsOther;
        }
    }
}
