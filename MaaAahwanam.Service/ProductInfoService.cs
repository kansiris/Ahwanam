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
        public GetProductsInfo_Result getProductsInfo_Result(int vid,string servicetype,int Subvid)
        {
            return vendorsOthersRepository.getProductsInfo(vid,servicetype, Subvid);
        }
    }
}
