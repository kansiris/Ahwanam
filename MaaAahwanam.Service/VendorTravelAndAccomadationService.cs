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
           vendorsTravelandAccomodation.Status = "Active";
           vendorsTravelandAccomodation.UpdatedDate  = DateTime.Now;
           vendorMaster.Status = "Active";
           vendorMaster.UpdatedDate = DateTime.Now;
           vendorMaster.ServicType = "Travel&Accommodation";
           vendorMaster = vendorMasterRepository.AddVendorMaster(vendorMaster);
           vendorsTravelandAccomodation.VendorMasterId = vendorMaster.Id;
           vendorsTravelandAccomodation = vendorsTravelandAccomodationRepository.AddTravelandAccomodation(vendorsTravelandAccomodation);
            userLogin.UserName = vendorMaster.EmailId;
            userLogin.Password = randomPassword.GenerateString();
            userLogin.UserType = "Vendor";
            userLogin.UpdatedBy = 2;
            userLogin.Status = "Active";
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
            vendorTravelandAccomodation.Status = "Active";
            vendorTravelandAccomodation.UpdatedDate = DateTime.Now;
            vendorMaster.Status = "Active";
            vendorMaster.UpdatedDate = DateTime.Now;
            vendorMaster.ServicType = "Travel&Accommodation";
            vendorMaster = vendorMasterRepository.UpdateVendorMaster(vendorMaster, masterid);
            vendorTravelandAccomodation = vendorsTravelandAccomodationRepository.UpdateTravelandAccomodation(vendorTravelandAccomodation, masterid,vid);
            return vendorTravelandAccomodation;
        }

        public VendorsTravelandAccomodation AddNewTravelandAccomodation(VendorsTravelandAccomodation vendorsTravelandAccomodation, Vendormaster vendorMaster)
        {
            vendorsTravelandAccomodation.Status = "Active";
            vendorsTravelandAccomodation.UpdatedDate = DateTime.Now;
            vendorsTravelandAccomodation.VendorMasterId = vendorMaster.Id;
            vendorsTravelandAccomodation = vendorsTravelandAccomodationRepository.AddTravelandAccomodation(vendorsTravelandAccomodation);
            return vendorsTravelandAccomodation;
        }
    }
}
