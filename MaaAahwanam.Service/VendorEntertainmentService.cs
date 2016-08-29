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
   public class VendorEntertainmentService
    {
        RandomPassword randomPassword = new RandomPassword();
        UserLoginRepository userLoginRepository = new UserLoginRepository();
        UserDetailsRepository userDetailsRepository = new UserDetailsRepository();
        UserLogin userLogin = new UserLogin();
        UserDetail userDetail = new UserDetail();
        VendormasterRepository vendorMasterRepository = new VendormasterRepository();
        VendorEntertainmentRepository vendorEntertainmentRespository = new VendorEntertainmentRepository();

        public VendorsEntertainment AddEntertainment(VendorsEntertainment vendorEntertainment, Vendormaster vendorMaster)
        {
            vendorEntertainment.Status = "Active";
            vendorEntertainment.UpdatedDate =  DateTime.Now;
            vendorMaster.Status = "Active";
            vendorMaster.UpdatedDate = DateTime.Now;
            vendorMaster.ServicType = "Entertainment";
            vendorMaster = vendorMasterRepository.AddVendorMaster(vendorMaster);
            vendorEntertainment.VendorMasterId = vendorMaster.Id;
            vendorEntertainment = vendorEntertainmentRespository.AddEntertainment(vendorEntertainment);
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
            if (vendorMaster.Id != 0 && vendorEntertainment.Id != 0 && userLogin.UserLoginId != 0 && userDetail.UserDetailId != 0)
            {
                return vendorEntertainment;
            }
            else
            {
                vendorEntertainment.Id = 0;
                return vendorEntertainment;
            }
        }

        public VendorsEntertainment GetVendorEntertainment(long id, long vid)
        {
            return vendorEntertainmentRespository.GetVendorEntertainment(id,vid);
        }

        public VendorsEntertainment UpdateEntertainment(VendorsEntertainment vendorsEntertainment, Vendormaster vendorMaster, long masterid, long vid)
        {
            vendorsEntertainment.Status = "Active";
            vendorsEntertainment.UpdatedDate = DateTime.Now;
            vendorMaster.Status = "Active";
            vendorMaster.UpdatedDate = DateTime.Now;
            vendorMaster.ServicType = "Entertainment";
            vendorMaster = vendorMasterRepository.UpdateVendorMaster(vendorMaster, masterid);
            vendorsEntertainment = vendorEntertainmentRespository.UpdateEntertainment(vendorsEntertainment, masterid,vid);
            return vendorsEntertainment;
        }

        public VendorsEntertainment AddNewEntertainment(VendorsEntertainment vendorsEntertainment, Vendormaster vendorMaster)
        {
            vendorsEntertainment.Status = "Active";
            vendorsEntertainment.UpdatedDate = DateTime.Now;
            vendorsEntertainment.VendorMasterId = vendorMaster.Id;
            vendorsEntertainment = vendorEntertainmentRespository.AddEntertainment(vendorsEntertainment);
            return vendorsEntertainment;
        }
    }
}
