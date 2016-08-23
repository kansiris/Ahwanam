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
        UserLogin userLogin = new UserLogin();
        VendormasterRepository vendorMasterRepository = new VendormasterRepository();
        VendorEventOrganiserRepository vendorEventOrganiserRepository = new VendorEventOrganiserRepository();
        public VendorsEventOrganiser AddEventOrganiser(VendorsEventOrganiser vendorEventOrganiser, Vendormaster vendorMaster)
        {
            vendorEventOrganiser.Status = "Active";
            vendorEventOrganiser.UpdatedDate =  DateTime.Now;
            vendorMaster.Status = "Active";
            vendorMaster.UpdatedDate = DateTime.Now;
            vendorMaster.ServicType = "EventOrganiser";
            vendorMaster = vendorMasterRepository.AddVendorMaster(vendorMaster);
            vendorEventOrganiser.VendorMasterId = vendorMaster.Id;
            vendorEventOrganiser = vendorEventOrganiserRepository.AddEventOrganiser(vendorEventOrganiser);
            userLogin.UserName = vendorMaster.EmailId;
            userLogin.Password = randomPassword.GenerateString();
            userLogin.UserType = "Vendor";
            userLogin.UpdatedBy = 2;
            userLogin.RegDate = DateTime.Now;
            userLogin.UpdatedDate = DateTime.Now;
            userLogin.Status = "Active";
            userLogin = userLoginRepository.AddVendorUserLogin(userLogin);
            if (vendorMaster.Id != 0 && vendorEventOrganiser.Id != 0 && userLogin.UserLoginId != 0)
            {
                return vendorEventOrganiser;
            }
            else
            {
                vendorEventOrganiser.Id = 0;
                return vendorEventOrganiser;
            }
        }
        public VendorsEventOrganiser GetVendorEventOrganiser(long id)
        {
            return vendorEventOrganiserRepository.GetVendorEventOrganiser(id);
        }

        public VendorsEventOrganiser UpdateEventOrganiser(VendorsEventOrganiser vendorsEventOrganiser, Vendormaster vendorMaster, long masterid)
        {
            vendorsEventOrganiser.Status = "Active";
            vendorsEventOrganiser.UpdatedDate = DateTime.Now;
            vendorMaster.Status = "Active";
            vendorMaster.UpdatedDate = DateTime.Now;
            vendorMaster.ServicType = "EventOrganiser";
            vendorMaster = vendorMasterRepository.UpdateVendorMaster(vendorMaster, masterid);
            vendorsEventOrganiser = vendorEventOrganiserRepository.UpdateEventOrganiser(vendorsEventOrganiser, masterid);
            return vendorsEventOrganiser;
        }
    }
}
