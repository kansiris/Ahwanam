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
   public class VendorWeddingCollectionService
    {
        RandomPassword randomPassword = new RandomPassword();
        UserLoginRepository userLoginRepository = new UserLoginRepository();
        UserLogin userLogin = new UserLogin();
        VendormasterRepository vendorMasterRepository = new VendormasterRepository();
        VendorWeddingCollectionsRepository vendorsWeddingCollectionsRepository = new VendorWeddingCollectionsRepository();
        public VendorsWeddingCollection AddWeddingCollection(VendorsWeddingCollection vendorsWeddingCollection, Vendormaster vendorMaster)
       {
           vendorsWeddingCollection.Status = "Active";
           vendorsWeddingCollection.UpdatedDate =  DateTime.Now;
           vendorMaster.Status = "Active";
           vendorMaster.UpdatedDate = DateTime.Now;
           vendorMaster.ServicType = "WeddingCollection";
           vendorMaster = vendorMasterRepository.AddVendorMaster(vendorMaster);
           vendorsWeddingCollection.VendorMasterId = vendorMaster.Id;
           vendorsWeddingCollection = vendorsWeddingCollectionsRepository.AddWeddingCollections(vendorsWeddingCollection);
            userLogin.UserName = vendorMaster.EmailId;
            userLogin.Password = randomPassword.GenerateString();
            userLogin.UserType = "Vendor";
            userLogin.UpdatedBy = 2;
            userLogin.RegDate = DateTime.Now;
            userLogin.Status = "Active";
            userLogin.UpdatedDate = DateTime.Now;
            userLogin = userLoginRepository.AddVendorUserLogin(userLogin);
            if (vendorMaster.Id != 0 && vendorsWeddingCollection.Id != 0 && userLogin.UserLoginId != 0)
            {
                return vendorsWeddingCollection;
            }
            else
            {
                vendorsWeddingCollection.Id = 0;
                return vendorsWeddingCollection;
            }
        }
        public VendorsWeddingCollection GetVendorWeddingCollection(long id)
        {
            return vendorsWeddingCollectionsRepository.GetVendorWeddingCollection(id);
        }

        public VendorsWeddingCollection UpdateWeddingCollection(VendorsWeddingCollection vendorsWeddingCollection, Vendormaster vendorMaster, long masterid)
        {
            vendorsWeddingCollection.Status = "Active";
            vendorsWeddingCollection.UpdatedDate = DateTime.Now;
            vendorMaster.Status = "Active";
            vendorMaster.UpdatedDate = DateTime.Now;
            vendorMaster.ServicType = "WeddingCollection";
            vendorMaster = vendorMasterRepository.UpdateVendorMaster(vendorMaster, masterid);
            vendorsWeddingCollection = vendorsWeddingCollectionsRepository.UpdateWeddingCollection(vendorsWeddingCollection, masterid);
            return vendorsWeddingCollection;
        }
    }
}
