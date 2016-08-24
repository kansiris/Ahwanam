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
    public class VendorDecoratorService
    {
        RandomPassword randomPassword = new RandomPassword();
        UserLoginRepository userLoginRepository = new UserLoginRepository();
        VendormasterRepository vendorMasterRepository = new VendormasterRepository();
        VendorsDecoratorRepository vendorsDecoratorRepository = new VendorsDecoratorRepository();
        UserLogin userLogin = new UserLogin();
        public VendorsDecorator AddDecorator(VendorsDecorator vendorsdecorator,Vendormaster vendorMaster)
        {
            vendorMaster.ServicType = "Decorators";
            vendorMaster.Status = "Active";
            vendorMaster.UpdatedDate =  DateTime.Now;
            vendorsdecorator.Status = "Active";
            vendorsdecorator.UpdatedDate = DateTime.Now;
            vendorMaster = vendorMasterRepository.AddVendorMaster(vendorMaster);
            vendorsdecorator.VendorMasterId = vendorMaster.Id;
            vendorsdecorator = vendorsDecoratorRepository.AddDecorator(vendorsdecorator);
            userLogin.UserName = vendorMaster.EmailId;
            userLogin.Password = randomPassword.GenerateString();
            userLogin.UserType = "Vendor";
            userLogin.UpdatedBy = 2;
            userLogin.RegDate = DateTime.Now;
            userLogin.UpdatedDate = DateTime.Now;
            userLogin.Status = "Active";
            userLogin = userLoginRepository.AddVendorUserLogin(userLogin);
            if (vendorMaster.Id != 0 && vendorsdecorator.Id != 0 && userLogin.UserLoginId != 0)
            {
                return vendorsdecorator;
            }
            else
            {
                vendorsdecorator.Id = 0;
                return vendorsdecorator;
            }
        }

        public VendorsDecorator GetVendorDecorator(long id,long vid)
        {
            return vendorsDecoratorRepository.GetVendorDecorator(id,vid);
        }

        public VendorsDecorator UpdateDecorator(VendorsDecorator vendorsDecorator, Vendormaster vendorMaster, long masterid,long vid)
        {
            vendorsDecorator.Status = "Active";
            vendorsDecorator.UpdatedDate = DateTime.Now;
            vendorMaster.Status = "Active";
            vendorMaster.UpdatedDate = DateTime.Now;
            vendorMaster.ServicType = "Decorators";
            vendorMaster = vendorMasterRepository.UpdateVendorMaster(vendorMaster, masterid);
            vendorsDecorator = vendorsDecoratorRepository.UpdateDecorator(vendorsDecorator, masterid,vid);
            return vendorsDecorator;
        }

        public VendorsDecorator AddNewDecorator(VendorsDecorator vendorsDecorator, Vendormaster vendorMaster)
        {
            vendorsDecorator.Status = "Active";
            vendorsDecorator.UpdatedDate = DateTime.Now;
            vendorsDecorator.VendorMasterId = vendorMaster.Id;
            vendorsDecorator = vendorsDecoratorRepository.AddDecorator(vendorsDecorator);
            return vendorsDecorator;
        }
    }
}
