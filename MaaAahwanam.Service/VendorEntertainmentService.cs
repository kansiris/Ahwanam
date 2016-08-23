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
   public class VendorEntertainmentService
    {
        RandomPassword randomPassword = new RandomPassword();
        UserLoginRepository userLoginRepository = new UserLoginRepository();
        UserLogin userLogin = new UserLogin();
        VendormasterRepository vendorMasterRepository = new VendormasterRepository();
        VendorEntertainmentRepository vendorEntertainmentRespository = new VendorEntertainmentRepository();

        public VendorsEntertainment AddEntertainment(VendorsEntertainment vendorEntertainment, Vendormaster vendorMaster)
        {
            vendorEntertainment.Status = "Active";
            vendorEntertainment.UpdatedDate =  DateTime.Now;
            vendorMaster.Status = "Active";
            vendorMaster.UpdatedDate = DateTime.Now;
            vendorMaster.ServicType = "Entertainment";
            vendorMaster = vendorMasterRepository.AddVendorMaster(vendorMaster);
            vendorEntertainment.VendorMasterId = vendorMaster.Id;
            vendorEntertainment = vendorEntertainmentRespository.AddEntertainment(vendorEntertainment);
            userLogin.UserName = vendorMaster.EmailId;
            userLogin.Password = randomPassword.GenerateString();
            userLogin.UserType = "Vendor";
            userLogin.UpdatedBy = 2;
            userLogin.RegDate = DateTime.Now;
            userLogin.UpdatedDate = DateTime.Now;
            userLogin.Status = "Active";
            userLogin = userLoginRepository.AddVendorUserLogin(userLogin);
            if (vendorMaster.Id != 0 && vendorEntertainment.Id != 0 && userLogin.UserLoginId != 0)
            {
                return vendorEntertainment;
            }
            else
            {
                vendorEntertainment.Id = 0;
                return vendorEntertainment;
            }
        }

        public VendorsEntertainment GetVendorEntertainment(long id)
        {
            return vendorEntertainmentRespository.GetVendorEntertainment(id);
        }

        public VendorsEntertainment UpdateEntertainment(VendorsEntertainment vendorsEntertainment, Vendormaster vendorMaster, long masterid)
        {
            vendorsEntertainment.Status = "Active";
            vendorsEntertainment.UpdatedDate = DateTime.Now;
            vendorMaster.Status = "Active";
            vendorMaster.UpdatedDate = DateTime.Now;
            vendorMaster.ServicType = "Entertainment";
            vendorMaster = vendorMasterRepository.UpdateVendorMaster(vendorMaster, masterid);
            vendorsEntertainment = vendorEntertainmentRespository.UpdateEntertainment(vendorsEntertainment, masterid);
            return vendorsEntertainment;
        }
    }
}
