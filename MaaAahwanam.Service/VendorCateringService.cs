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
   public class VendorCateringService
    {
        RandomPassword randomPassword = new RandomPassword();
        UserLoginRepository userLoginRepository = new UserLoginRepository();
        VendormasterRepository vendorMasterRepository = new VendormasterRepository();
        VendorCateringRepository vendorCateringRespository = new VendorCateringRepository();
        UserDetailsRepository userDetailsRepository = new UserDetailsRepository();
        UserLogin userLogin = new UserLogin();
        UserDetail userDetail = new UserDetail();
        public VendorsCatering AddCatering(VendorsCatering vendorCatering, Vendormaster vendorMaster)
        {
            vendorCatering.Status = "Active";
            vendorCatering.UpdatedDate =  DateTime.Now;
            vendorMaster.Status = "Active";
            vendorMaster.UpdatedDate = DateTime.Now;
            vendorMaster.ServicType = "Catering";
            vendorMaster = vendorMasterRepository.AddVendorMaster(vendorMaster);
            vendorCatering.VendorMasterId = vendorMaster.Id;
            vendorCatering = vendorCateringRespository.AddCatering(vendorCatering);
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
            if (vendorMaster.Id != 0 && vendorCatering.Id != 0 && userLogin.UserLoginId != 0 && userDetail.UserDetailId != 0)
            {
                return vendorCatering;
            }
            else
            {
                vendorCatering.Id = 0;
                return vendorCatering;
            }
            
        }
        public VendorsCatering GetVendorCatering(long id,long vid)
        {
            return vendorCateringRespository.GetVendorsCatering(id,vid);
        }

        public VendorsCatering UpdatesCatering(VendorsCatering vendorsCatering, Vendormaster vendorMaster, long masterid,long vid)
        {
            vendorsCatering.Status = "Active";
            vendorsCatering.UpdatedDate = DateTime.Now;
            vendorMaster.Status = "Active";
            vendorMaster.UpdatedDate = DateTime.Now;
            vendorMaster.ServicType = "Catering";
            vendorMaster = vendorMasterRepository.UpdateVendorMaster(vendorMaster, masterid);
            vendorsCatering = vendorCateringRespository.UpdatesCatering(vendorsCatering, masterid,vid);
            return vendorsCatering;
        }

        public VendorsCatering AddNewCatering(VendorsCatering vendorsCatering, Vendormaster vendorMaster)
        {
            vendorsCatering.Status = "Active";
            vendorsCatering.UpdatedDate = DateTime.Now;
            vendorsCatering.VendorMasterId = vendorMaster.Id;
            vendorsCatering = vendorCateringRespository.AddCatering(vendorsCatering);
            return vendorsCatering;
        }
    }
}
