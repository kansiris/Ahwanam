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
   public class VendorEventOrganiserService
    {
        RandomPassword randomPassword = new RandomPassword();
        UserLoginRepository userLoginRepository = new UserLoginRepository();
        UserDetailsRepository userDetailsRepository = new UserDetailsRepository();
        UserLogin userLogin = new UserLogin();
        UserDetail userDetail = new UserDetail();
        VendormasterRepository vendorMasterRepository = new VendormasterRepository();
        VendorEventOrganiserRepository vendorEventOrganiserRepository = new VendorEventOrganiserRepository();
        public VendorsEventOrganiser AddEventOrganiser(VendorsEventOrganiser vendorEventOrganiser, Vendormaster vendorMaster)
        {
            string updateddate = DateTime.UtcNow.ToShortDateString();
            vendorEventOrganiser.Status = "Active";
            vendorEventOrganiser.UpdatedDate =  Convert.ToDateTime(updateddate);
            vendorMaster.Status = "Active";
            vendorMaster.UpdatedDate = Convert.ToDateTime(updateddate);
            vendorMaster.ServicType = "EventOrganiser";
            vendorMaster = vendorMasterRepository.AddVendorMaster(vendorMaster);
            vendorEventOrganiser.VendorMasterId = vendorMaster.Id;
            vendorEventOrganiser = vendorEventOrganiserRepository.AddEventOrganiser(vendorEventOrganiser);
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
            if (vendorMaster.Id != 0 && vendorEventOrganiser.Id != 0 && userLogin.UserLoginId != 0 && userDetail.UserDetailId != 0)
            {
                return vendorEventOrganiser;
            }
            else
            {
                vendorEventOrganiser.Id = 0;
                return vendorEventOrganiser;
            }
        }
        public VendorsEventOrganiser GetVendorEventOrganiser(long id, long vid)
        {
            return vendorEventOrganiserRepository.GetVendorEventOrganiser(id,vid);
        }

        public VendorsEventOrganiser UpdateEventOrganiser(VendorsEventOrganiser vendorsEventOrganiser, Vendormaster vendorMaster, long masterid, long vid)
        {
            string updateddate = DateTime.UtcNow.ToShortDateString();
            vendorsEventOrganiser.Status = "Active";
            vendorsEventOrganiser.UpdatedDate = Convert.ToDateTime(updateddate);
            vendorMaster.Status = "Active";
            vendorMaster.UpdatedDate = Convert.ToDateTime(updateddate);
            vendorMaster.ServicType = "EventOrganiser";
            vendorMaster = vendorMasterRepository.UpdateVendorMaster(vendorMaster, masterid);
            vendorsEventOrganiser = vendorEventOrganiserRepository.UpdateEventOrganiser(vendorsEventOrganiser, masterid,vid);
            return vendorsEventOrganiser;
        }

        public VendorsEventOrganiser AddNewEventOrganiser(VendorsEventOrganiser vendorsEventOrganiser, Vendormaster vendorMaster)
        {
            string updateddate = DateTime.UtcNow.ToShortDateString();
            vendorsEventOrganiser.Status = "Active";
            vendorsEventOrganiser.UpdatedDate = Convert.ToDateTime(updateddate);
            vendorsEventOrganiser.VendorMasterId = vendorMaster.Id;
            vendorsEventOrganiser = vendorEventOrganiserRepository.AddEventOrganiser(vendorsEventOrganiser);
            return vendorsEventOrganiser;
        }
    }
}
