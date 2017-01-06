using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaaAahwanam.Repository.db;
using MaaAahwanam.Models;
using MaaAahwanam.Repository;

namespace MaaAahwanam.Service
{
    public class ProductInfoService
    {
        VendorsOthersRepository vendorsOthersRepository = new VendorsOthersRepository();
        VendorVenueRepository vendorVenueRepository = new VendorVenueRepository();
        OrderdetailsRepository orderdetailsRepository = new OrderdetailsRepository();
        
        public GetProductsInfo_Result getProductsInfo_Result(int vid,string servicetype,int Subvid)
        {
            return vendorsOthersRepository.getProductsInfo(vid,servicetype, Subvid);
        }
        public SP_dealsinfo_Result getDealsInfo_Result(int vid, string servicetype, int Subvid, int did)
        {
            return vendorsOthersRepository.getDealInfo(vid, servicetype, Subvid,did);
        }

        public List<OrderDetail> GetCount(long vid, long subid, string servicetype)
        {
            return orderdetailsRepository.GetCount(vid,subid,servicetype);
        }

        public long? ratingscount(long vid, long subid, string servicetype)
        {
            return orderdetailsRepository.ratingscount(vid, subid, servicetype);
        }

        public List<SP_Amenities_Result> GetAmenities(long subid, string type)
        {
            return orderdetailsRepository.GetAmenities(subid, type);
        }
        
    }
}
