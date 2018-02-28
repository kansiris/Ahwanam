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

        public Vendormaster GetVendorByEmail(string emailid)
        {
            return vendormasterRepository.GetVendorByEmail(emailid);
        }
        public List<dynamic> GetVendorLocations()
        {
            return vendormasterRepository.VendormasterList().Select(m => m.Landmark).ToList<dynamic>();
        }
        public List<dynamic> GetVendorname()
        {
            return vendormasterRepository.VendormasterList().Select(m => m.BusinessName).ToList<dynamic>();
        }
        public List<dynamic> GetVendorword()
        {
            var l1 = vendormasterRepository.VendormasterList().Select(i => i.BusinessName + "," + i.Address +  "," + i.ServicType);
           return l1.ToList<dynamic>();
          }
    }
}
