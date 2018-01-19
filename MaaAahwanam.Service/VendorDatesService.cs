using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaaAahwanam.Models;
using MaaAahwanam.Repository.db;
using MaaAahwanam.Repository;

namespace MaaAahwanam.Service
{
    public class VendorDatesService
    {
        VendorDatesRepository vendorDatesRepository = new VendorDatesRepository();

        public VendorDates SaveVendorDates(VendorDates vendorDates)
        {
            return vendorDatesRepository.SaveVendorDates(vendorDates);
        }

        public List<VendorDates> GetDates(long vendorid, long subid)
        {
            return vendorDatesRepository.GetDates(vendorid, subid);
        }

        public string removedates(long id)
        {
            return vendorDatesRepository.removedates(id);
        }

        public VendorDates UpdatesVendorDates(VendorDates vendorDates, long id)
        {
            return vendorDatesRepository.UpdatesVendorDates(vendorDates, id);
        }
    }
}
