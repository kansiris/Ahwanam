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
        RandomPassword randomPassword = new RandomPassword();
        UserLoginRepository userLoginRepository = new UserLoginRepository();
        UserLogin userLogin = new UserLogin();
        VendormasterRepository vendorMasterRepository = new VendormasterRepository();
        VendorsPhotographyRepository vendorsPhotographyRepository = new VendorsPhotographyRepository();
        public VendorsPhotography AddPhotography(VendorsPhotography vendorPhotography, Vendormaster vendorMaster)
       {
           vendorPhotography.Status = "Active";
           vendorPhotography.UpdatedDate =  DateTime.Now;
           vendorMaster.Status = "Active";
           vendorMaster.UpdatedDate = DateTime.Now;
           vendorMaster.ServicType = "Photography";
           vendorMaster = vendorMasterRepository.AddVendorMaster(vendorMaster);
           vendorPhotography.VendorMasterId = vendorMaster.Id;
           vendorPhotography = vendorsPhotographyRepository.AddPhotography(vendorPhotography);
            userLogin.UserName = vendorMaster.EmailId;
            userLogin.Password = randomPassword.GenerateString();
            userLogin.UserType = "Vendor";
            userLogin.UpdatedBy = 2;
            userLogin.Status = "Active";
            userLogin.RegDate = DateTime.Now;
            userLogin.UpdatedDate = DateTime.Now;
            userLogin = userLoginRepository.AddVendorUserLogin(userLogin);
            if (vendorMaster.Id != 0 && vendorPhotography.Id != 0 && userLogin.UserLoginId != 0)
            {
                return vendorPhotography;
            }
            else
            {
                vendorPhotography.Id = 0;
                return vendorPhotography;
            }
        }
        public VendorsPhotography GetVendorPhotography(long id,long vid)
        {
            return vendorsPhotographyRepository.GetVendorsPhotography(id,vid);
        }

        public VendorsPhotography UpdatesPhotography(VendorsPhotography vendorsPhotography, Vendormaster vendorMaster, long masterid,long vid)
        {
            vendorsPhotography.Status = "Active";
            vendorsPhotography.UpdatedDate = DateTime.Now;
            vendorMaster.Status = "Active";
            vendorMaster.UpdatedDate = DateTime.Now;
            vendorMaster.ServicType = "Photography";
            vendorMaster = vendorMasterRepository.UpdateVendorMaster(vendorMaster, masterid);
            vendorsPhotography =  vendorsPhotographyRepository.UpdatesPhotography(vendorsPhotography, masterid,vid);
            return vendorsPhotography;
        }

        public VendorsPhotography AddNewPhotography(VendorsPhotography vendorsPhotography, Vendormaster vendorMaster)
        {
            vendorsPhotography.Status = "Active";
            vendorsPhotography.UpdatedDate = DateTime.Now;
            vendorsPhotography.VendorMasterId = vendorMaster.Id;
            vendorsPhotography = vendorsPhotographyRepository.AddPhotography(vendorsPhotography);
            return vendorsPhotography;
        }
    }
}
