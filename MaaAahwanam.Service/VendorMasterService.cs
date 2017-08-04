using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaaAahwanam.Repository.db;
using MaaAahwanam.Models;

namespace MaaAahwanam.Service
{
    public class VendorMasterService
    {
        VendormasterRepository vendormasterRepository = new VendormasterRepository();
        public Vendormaster GetVendor(long id)
        {
            return vendormasterRepository.GetVendor(id);
        }

        public int checkemail(string emailid)
        {
            return vendormasterRepository.checkemail(emailid);
        }

        public Vendormaster GetVendorServiceType(long id)
        {
            return vendormasterRepository.GetVendorServiceType(id);
        }

        public Vendormaster UpdateVendorMaster(Vendormaster vendorMaster, long id)
        {
            return vendormasterRepository.UpdateVendorMaster(vendorMaster,id);
        }
    }
}
