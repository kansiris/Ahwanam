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
   public class VendorOthersService
    {
        RandomPassword randomPassword = new RandomPassword();
        UserLoginRepository userLoginRepository = new UserLoginRepository();
        UserLogin userLogin = new UserLogin();
        VendormasterRepository vendorMasterRepository = new VendormasterRepository();
        VendorOthersRepository vendorOthersRepository = new VendorOthersRepository();
        public VendorsOther AddOther(VendorsOther vendorOther, Vendormaster vendorMaster)
       {
           vendorOther.Status = "Active";
           vendorOther.UpdatedDate =  DateTime.Now;
           vendorMaster.Status = "Active";
           vendorMaster.UpdatedDate = DateTime.Now;
           vendorMaster.ServicType = "Other";
           vendorMaster = vendorMasterRepository.AddVendorMaster(vendorMaster);
           vendorOther.VendorMasterId = vendorMaster.Id;
           vendorOther = vendorOthersRepository.AddOthers(vendorOther);
            userLogin.UserName = vendorMaster.EmailId;
            userLogin.Password = randomPassword.GenerateString();
            userLogin.UserType = "Vendor";
            userLogin.Status = "Active";
            userLogin.UpdatedBy = 2;
            userLogin.RegDate = DateTime.Now;
            userLogin.UpdatedDate = DateTime.Now;
            userLogin = userLoginRepository.AddVendorUserLogin(userLogin);
            if (vendorMaster.Id != 0 && vendorOther.Id != 0 && userLogin.UserLoginId != 0)
            {
                return vendorOther;
            }
            else
            {
                vendorOther.Id = 0;
                return vendorOther;
            }
        }

        public VendorsOther GetVendorOther(long id)
        {
            return vendorOthersRepository.GetVendorOthers(id);
        }

        public VendorsOther UpdateOther(VendorsOther vendorOther, Vendormaster vendorMaster, long masterid)
        {
            vendorOther.Status = "Active";
            vendorOther.UpdatedDate = DateTime.Now;
            vendorMaster.Status = "Active";
            vendorMaster.UpdatedDate = DateTime.Now;
            vendorMaster.ServicType = "Other";
            vendorMaster = vendorMasterRepository.UpdateVendorMaster(vendorMaster, masterid);
            vendorOther = vendorOthersRepository.UpdateOthers(vendorOther, masterid);
            return vendorOther;
        }
    }
}
