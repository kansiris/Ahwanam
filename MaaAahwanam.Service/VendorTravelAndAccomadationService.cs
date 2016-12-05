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
   public class VendorTravelAndAccomadationService
    {
        RandomPassword randomPassword = new RandomPassword();
        UserLoginRepository userLoginRepository = new UserLoginRepository();
        UserDetailsRepository userDetailsRepository = new UserDetailsRepository();
        UserLogin userLogin = new UserLogin();
        UserDetail userDetail = new UserDetail();
        VendormasterRepository vendorMasterRepository = new VendormasterRepository();
        VendorsTravelandAccomodationRepository vendorsTravelandAccomodationRepository = new VendorsTravelandAccomodationRepository();
        public VendorsTravelandAccomodation AddTravelAndAccomadation(VendorsTravelandAccomodation vendorsTravelandAccomodation, Vendormaster vendorMaster)
       {
            string updateddate = DateTime.UtcNow.ToShortDateString();
            vendorsTravelandAccomodation.Status = "Active";
           vendorsTravelandAccomodation.UpdatedDate  = Convert.ToDateTime(updateddate);
           vendorMaster.Status = "Active";
           vendorMaster.UpdatedDate = Convert.ToDateTime(updateddate);
           vendorMaster.ServicType = "Travel&Accommodation";
           vendorMaster = vendorMasterRepository.AddVendorMaster(vendorMaster);
           vendorsTravelandAccomodation.VendorMasterId = vendorMaster.Id;
           vendorsTravelandAccomodation = vendorsTravelandAccomodationRepository.AddTravelandAccomodation(vendorsTravelandAccomodation);
            userLogin.UserName = vendorMaster.EmailId;
            userLogin.Password = randomPassword.GenerateString();
            userLogin.UserType = "Vendor";
            userLogin.UpdatedBy = 2;
            userLogin.Status = "Active";
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
            if (vendorMaster.Id != 0 && vendorsTravelandAccomodation.Id != 0 && userLogin.UserLoginId != 0 && userDetail.UserDetailId != 0)
            {
                return vendorsTravelandAccomodation;
            }
            else
            {
                vendorsTravelandAccomodation.Id = 0;
                return vendorsTravelandAccomodation;
            }
        }

        public VendorsTravelandAccomodation GetVendorTravelandAccomodation(long id,long vid)
        {
            return vendorsTravelandAccomodationRepository.GetVendorTravelandAccomodation(id,vid);
        }

        public VendorsTravelandAccomodation UpdateTravelandAccomodation(VendorsTravelandAccomodation vendorTravelandAccomodation, Vendormaster vendorMaster, long masterid,long vid)
        {
            string updateddate = DateTime.UtcNow.ToShortDateString();
            vendorTravelandAccomodation.Status = "Active";
            vendorTravelandAccomodation.UpdatedDate = Convert.ToDateTime(updateddate);
            vendorMaster.Status = "Active";
            vendorMaster.UpdatedDate = Convert.ToDateTime(updateddate);
            vendorMaster.ServicType = "Travel&Accommodation";
            vendorMaster = vendorMasterRepository.UpdateVendorMaster(vendorMaster, masterid);
            vendorTravelandAccomodation = vendorsTravelandAccomodationRepository.UpdateTravelandAccomodation(vendorTravelandAccomodation, masterid,vid);
            return vendorTravelandAccomodation;
        }

        public VendorsTravelandAccomodation AddNewTravelandAccomodation(VendorsTravelandAccomodation vendorsTravelandAccomodation, Vendormaster vendorMaster)
        {
            string updateddate = DateTime.UtcNow.ToShortDateString();
            vendorsTravelandAccomodation.Status = "Active";
            vendorsTravelandAccomodation.UpdatedDate = Convert.ToDateTime(updateddate);
            vendorsTravelandAccomodation.VendorMasterId = vendorMaster.Id;
            vendorsTravelandAccomodation = vendorsTravelandAccomodationRepository.AddTravelandAccomodation(vendorsTravelandAccomodation);
            return vendorsTravelandAccomodation;
        }
    }
}
