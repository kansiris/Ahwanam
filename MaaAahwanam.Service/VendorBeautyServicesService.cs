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
   public class VendorBeautyServicesService
    {
        RandomPassword randomPassword = new RandomPassword();
        UserLoginRepository userLoginRepository = new UserLoginRepository();
        VendormasterRepository vendorMasterRepository = new VendormasterRepository();
        VendorsBeautyServiceRepository vendorBeautyServiceRespository = new VendorsBeautyServiceRepository();
        UserDetailsRepository userDetailsRepository = new UserDetailsRepository();
        UserLogin userLogin = new UserLogin();
        UserDetail userDetail = new UserDetail();
        public VendorsBeautyService AddBeautyService(VendorsBeautyService vendorBeautyService,Vendormaster vendorMaster)
        {
            string updateddate = DateTime.UtcNow.ToShortDateString();
            vendorBeautyService.UpdatedDate = Convert.ToDateTime(updateddate);
            vendorBeautyService.Status = "Active";
            vendorMaster.UpdatedDate = userLogin.RegDate = userLogin.UpdatedDate = Convert.ToDateTime(updateddate);
            vendorMaster.Status =  userLogin.Status= "Active";
            vendorMaster.ServicType = "BeautyServices";
            vendorMaster = vendorMasterRepository.AddVendorMaster(vendorMaster);
            vendorBeautyService.VendorMasterId = vendorMaster.Id;
            vendorBeautyService = vendorBeautyServiceRespository.AddBeautyService(vendorBeautyService);
            userLogin.UserName = vendorMaster.EmailId;
            userLogin.Password = randomPassword.GenerateString();
            userLogin.UserType = "Vendor";
            userLogin.UpdatedBy = 2;
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
            if (vendorMaster.Id != 0 && vendorBeautyService.Id != 0 && userLogin.UserLoginId != 0 && userDetail.UserDetailId != 0)
            {
                return vendorBeautyService;
            }
            else
            {
                vendorBeautyService.Id = 0;
                return vendorBeautyService;
            }
        }
        public VendorsBeautyService GetVendorBeautyService(long id,long vid)
        {
            return vendorBeautyServiceRespository.GetVendorsBeautyService(id,vid);
        }

        public VendorsBeautyService UpdatesBeautyService(VendorsBeautyService vendorsBeautyService, Vendormaster vendorMaster, long masterid,long vid)
        {
            string updateddate = DateTime.UtcNow.ToShortDateString();
            vendorsBeautyService.Status = "Active";
            vendorsBeautyService.UpdatedDate = Convert.ToDateTime(updateddate);
            vendorMaster.Status = "Active";
            vendorMaster.UpdatedDate = Convert.ToDateTime(updateddate);
            vendorMaster.ServicType = "BeautyServices";
            vendorMaster = vendorMasterRepository.UpdateVendorMaster(vendorMaster, masterid);
            vendorsBeautyService = vendorBeautyServiceRespository.UpdatesBeautyService(vendorsBeautyService, masterid,vid);
            return vendorsBeautyService;
        }
        public VendorsBeautyService AddNewBeautyService(VendorsBeautyService vendorsBeautyService, Vendormaster vendorMaster)
        {
            string updateddate = DateTime.UtcNow.ToShortDateString();
            vendorsBeautyService.Status = "Active";
            vendorsBeautyService.UpdatedDate = Convert.ToDateTime(updateddate);
            vendorsBeautyService.VendorMasterId = vendorMaster.Id;
            vendorsBeautyService = vendorBeautyServiceRespository.AddBeautyService(vendorsBeautyService);
            return vendorsBeautyService;
        }
    }
}
