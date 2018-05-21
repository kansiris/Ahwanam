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
    public class VendorPhotographyService
    {
        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        RandomPassword randomPassword = new RandomPassword();
        UserLoginRepository userLoginRepository = new UserLoginRepository();
        UserDetailsRepository userDetailsRepository = new UserDetailsRepository();
        UserLogin userLogin = new UserLogin();
        UserDetail userDetail = new UserDetail();
        VendormasterRepository vendorMasterRepository = new VendormasterRepository();
        VendorsPhotographyRepository vendorsPhotographyRepository = new VendorsPhotographyRepository();
        public VendorsPhotography AddPhotography(VendorsPhotography vendorPhotography, Vendormaster vendorMaster)
        {
            DateTime updateddate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
            vendorPhotography.Status = "Active";
            vendorPhotography.UpdatedDate = updateddate;
            vendorMaster.Status = "Active";
            vendorMaster.UpdatedDate = updateddate;
            vendorMaster.ServicType = "Photography";
            vendorMaster = vendorMasterRepository.AddVendorMaster(vendorMaster);
            vendorPhotography.VendorMasterId = vendorMaster.Id;
            vendorPhotography = vendorsPhotographyRepository.AddPhotography(vendorPhotography);
            userLogin.UserName = vendorMaster.EmailId;
            userLogin.Password = randomPassword.GenerateString();
            userLogin.UserType = "Vendor";
            userLogin.UpdatedBy = 2;
            userLogin.Status = "Active";
            userLogin.RegDate = updateddate;
            userLogin.UpdatedDate = updateddate;
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
            userDetail.UpdatedDate = updateddate;
            userDetail.AlternativeEmailID = vendorMaster.EmailId;
            userDetail.Landmark = vendorMaster.Landmark;
            userDetail = userDetailsRepository.AddUserDetails(userDetail);
            if (vendorMaster.Id != 0 && vendorPhotography.Id != 0 && userLogin.UserLoginId != 0 && userDetail.UserDetailId != 0)
            {
                return vendorPhotography;
            }
            else
            {
                vendorPhotography.Id = 0;
                return vendorPhotography;
            }
        }
        public VendorsPhotography GetVendorPhotography(long id, long vid)
        {
            return vendorsPhotographyRepository.GetVendorsPhotography(id, vid);
        }

        public VendorsPhotography UpdatesPhotography(VendorsPhotography vendorsPhotography, Vendormaster vendorMaster, long masterid, long vid)
        {
            DateTime updateddate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
            vendorsPhotography.Status = "Active";
            vendorsPhotography.UpdatedDate = updateddate;
            vendorMaster.Status = "Active";
            vendorMaster.UpdatedDate = updateddate;
            vendorMaster.ServicType = "Photography";
            vendorMaster = vendorMasterRepository.UpdateVendorMaster(vendorMaster, masterid);
            vendorsPhotography = vendorsPhotographyRepository.UpdatesPhotography(vendorsPhotography, masterid, vid);
            return vendorsPhotography;
        }


        public VendorsPhotography ActivePhotography(VendorsPhotography vendorsPhotography, Vendormaster vendorMaster, long masterid, long vid)
        {
            DateTime updateddate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
            //vendorsPhotography.Status = "Active";
            vendorsPhotography.UpdatedDate = updateddate;
            //vendorMaster.Status = "Active";
            vendorMaster.UpdatedDate = updateddate;
            vendorMaster.ServicType = "Photography";
            vendorMaster = vendorMasterRepository.UpdateVendorMaster(vendorMaster, masterid);
            vendorsPhotography = vendorsPhotographyRepository.UpdatesPhotography(vendorsPhotography, masterid, vid);
            return vendorsPhotography;
        }
        public VendorsPhotography InActivePhotography(VendorsPhotography vendorsPhotography, Vendormaster vendorMaster, long masterid, long vid)
        {
            DateTime updateddate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
            //vendorsPhotography.Status = "Active";
            vendorsPhotography.UpdatedDate = updateddate;
            //vendorMaster.Status = "Active";
            vendorMaster.UpdatedDate = updateddate;
            vendorMaster.ServicType = "Photography";
            vendorMaster = vendorMasterRepository.UpdateVendorMaster(vendorMaster, masterid);
            vendorsPhotography = vendorsPhotographyRepository.UpdatesPhotography(vendorsPhotography, masterid, vid);
            return vendorsPhotography;
        }

        public VendorsPhotography AddNewPhotography(VendorsPhotography vendorsPhotography, Vendormaster vendorMaster)
        {
            DateTime updateddate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
            vendorsPhotography.Status = "Active";
            vendorsPhotography.UpdatedDate = updateddate;
            vendorsPhotography.VendorMasterId = vendorMaster.Id;
            vendorsPhotography = vendorsPhotographyRepository.AddPhotography(vendorsPhotography);
            return vendorsPhotography;
        }
    }
}
