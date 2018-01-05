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
            string updateddate = DateTime.UtcNow.ToShortDateString();
            vendorCatering.Status = "Active";
            vendorCatering.UpdatedDate =  Convert.ToDateTime(updateddate);
            vendorMaster.Status = "Active";
            vendorMaster.UpdatedDate = Convert.ToDateTime(updateddate);
            vendorMaster.ServicType = "Catering";
            vendorMaster = vendorMasterRepository.AddVendorMaster(vendorMaster);
            vendorCatering.VendorMasterId = vendorMaster.Id;
            vendorCatering = vendorCateringRespository.AddCatering(vendorCatering);
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

        public VendorsCatering activeCatering(VendorsCatering vendorsCatering, Vendormaster vendorMaster, long masterid, long vid)
        {
            string updateddate = DateTime.UtcNow.ToShortDateString();
            vendorsCatering.Status = "Active";
            vendorsCatering.UpdatedDate = Convert.ToDateTime(updateddate);
            vendorMaster.Status = "Active";
            vendorMaster.UpdatedDate = Convert.ToDateTime(updateddate);
            vendorMaster.ServicType = "Catering";
            vendorMaster = vendorMasterRepository.UpdateVendorMaster(vendorMaster, masterid);
            vendorsCatering = vendorCateringRespository.UpdatesCatering(vendorsCatering, masterid, vid);
            return vendorsCatering;
        }

        public VendorsCatering InactiveCatering(VendorsCatering vendorsCatering, Vendormaster vendorMaster, long masterid, long vid)
        {
            string updateddate = DateTime.UtcNow.ToShortDateString();
            vendorsCatering.Status = "InActive";
            vendorsCatering.UpdatedDate = Convert.ToDateTime(updateddate);
            vendorMaster.Status = "InActive";
            vendorMaster.UpdatedDate = Convert.ToDateTime(updateddate);
            vendorMaster.ServicType = "Catering";
            vendorMaster = vendorMasterRepository.UpdateVendorMaster(vendorMaster, masterid);
            vendorsCatering = vendorCateringRespository.UpdatesCatering(vendorsCatering, masterid, vid);
            return vendorsCatering;
        }


        public VendorsCatering UpdatesCatering(VendorsCatering vendorsCatering, Vendormaster vendorMaster, long masterid,long vid)
        {
            string updateddate = DateTime.UtcNow.ToShortDateString();
            vendorsCatering.Status = "Active";
            vendorsCatering.UpdatedDate = Convert.ToDateTime(updateddate);
            vendorMaster.Status = "Active";
            vendorMaster.UpdatedDate = Convert.ToDateTime(updateddate);
            vendorMaster.ServicType = "Catering";
            vendorMaster = vendorMasterRepository.UpdateVendorMaster(vendorMaster, masterid);
            vendorsCatering = vendorCateringRespository.UpdatesCatering(vendorsCatering, masterid,vid);
            return vendorsCatering;
        }

        public VendorsCatering AddNewCatering(VendorsCatering vendorsCatering, Vendormaster vendorMaster)
        {
            string updateddate = DateTime.UtcNow.ToShortDateString();
            vendorsCatering.Status = "Active";
            vendorsCatering.UpdatedDate = Convert.ToDateTime(updateddate);
            vendorsCatering.VendorMasterId = vendorMaster.Id;
            vendorsCatering = vendorCateringRespository.AddCatering(vendorsCatering);
            return vendorsCatering;
        }
    }
}
