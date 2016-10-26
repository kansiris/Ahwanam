using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaaAahwanam.Repository.db;
using MaaAahwanam.Models;

namespace MaaAahwanam.Service
{
    public class AllVendorsService
    {
        public List<dynamic> VendorsList()
        {
            VendormasterRepository vendormasterRepository = new VendormasterRepository();
            var l1 = vendormasterRepository.VendormasterList().Select(i => i.Landmark + "," + i.City);
            return l1.ToList<dynamic>();
        }
    }
}
