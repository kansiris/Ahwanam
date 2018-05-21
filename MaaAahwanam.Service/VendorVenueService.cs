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
    public class VendorVenueService
    {
        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        RandomPassword randomPassword = new RandomPassword();
        UserLoginRepository userLoginRepository = new UserLoginRepository();
        UserDetailsRepository userDetailsRepository = new UserDetailsRepository();
        UserLogin userLogin = new UserLogin();
        UserDetail userDetail = new UserDetail();
        VendormasterRepository vendorMasterRepository = new VendormasterRepository();
        VendorVenueRepository vendorVenueRepository = new VendorVenueRepository();
        public VendorVenue AddVenue(VendorVenue vendorVenue, Vendormaster vendorMaster)
        {
            DateTime updateddate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
            vendorVenue.Status = "Active";
            vendorVenue.UpdatedDate = updateddate;
            vendorMaster.Status = "Active";
            vendorMaster.UpdatedDate = updateddate;
            vendorMaster.ServicType = "Venue";
            vendorMaster = vendorMasterRepository.AddVendorMaster(vendorMaster);
            vendorVenue.VendorMasterId = vendorMaster.Id;
            if (vendorVenue.Food == "Veg")
            {
                vendorVenue.NonVegLunchCost = 0;
                vendorVenue.NonVegDinnerCost = 0;
            }
            else if (vendorVenue.Food == "Non-Veg")
            {
                vendorVenue.VegLunchCost = 0;
                vendorVenue.VegDinnerCost = 0;
            }
            vendorVenue = vendorVenueRepository.AddVenue(vendorVenue);
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
            if (vendorMaster.Id != 0 && vendorVenue.Id != 0 && userLogin.UserLoginId != 0 && userDetail.UserDetailId != 0)
            {
                return vendorVenue;
            }
            else
            {
                vendorVenue.Id = 0;
                return vendorVenue;
            }
        }

        public VendorVenue GetVendorVenue(long id, long vid)
        {
            return vendorVenueRepository.GetVendorVenue(id, vid);
        }

        public VendorVenue UpdateVenue(VendorVenue vendorVenue, Vendormaster vendorMaster, long masterid, long vid)
        {
            DateTime updateddate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
            //vendorVenue.Status = "Active";
            vendorVenue.UpdatedDate = updateddate;
            //vendorMaster.Status = "Active";
            vendorMaster.UpdatedDate = updateddate;
            vendorMaster.ServicType = "Venue";
            vendorMaster = vendorMasterRepository.UpdateVendorMaster(vendorMaster, masterid);
            vendorVenue = vendorVenueRepository.UpdateVenue(vendorVenue, masterid, vid);
            return vendorVenue;
        }
        public VendorVenue activeVenue(VendorVenue vendorVenue, Vendormaster vendorMaster, long masterid, long vid)
        {
            DateTime updateddate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
            // vendorVenue.Status = "Active";
            vendorVenue.UpdatedDate = updateddate;
            // vendorMaster.Status = "Active";
            vendorMaster.UpdatedDate = updateddate;
            vendorMaster.ServicType = "Venue";
            vendorMaster = vendorMasterRepository.UpdateVendorMaster(vendorMaster, masterid);
            vendorVenue = vendorVenueRepository.UpdateVenue(vendorVenue, masterid, vid);
            return vendorVenue;
        }

        public VendorVenue inactiveVenue(VendorVenue vendorVenue, Vendormaster vendorMaster, long masterid, long vid)
        {
            DateTime updateddate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
            //    vendorVenue.Status = "InActive";
            vendorVenue.UpdatedDate = updateddate;
            //  vendorMaster.Status = "InActive";
            vendorMaster.UpdatedDate = updateddate;
            vendorMaster.ServicType = "Venue";
            vendorMaster = vendorMasterRepository.UpdateVendorMaster(vendorMaster, masterid);
            vendorVenue = vendorVenueRepository.UpdateVenue(vendorVenue, masterid, vid);
            return vendorVenue;
        }
        public VendorVenue AddNewVenue(VendorVenue vendorVenue, Vendormaster vendorMaster)
        {
            DateTime updateddate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
            vendorVenue.Status = "Active";
            vendorVenue.UpdatedDate = updateddate;
            vendorVenue.VendorMasterId = vendorMaster.Id;
            vendorVenue = vendorVenueRepository.AddVenue(vendorVenue);
            return vendorVenue;
        }

        public VendorVenue GetVendorType(long id)
        {
            return vendorVenueRepository.GetVendorType(id);
        }
    }
}
