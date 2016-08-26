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
    public class ProductService
    {
        VendorsOthersRepository vendorsOthersRepository = new VendorsOthersRepository();
        public List<GetProducts_Result> GetProducts_Results(string Param, int VID,string servicetypesType, string servicetypeloc, string servicetypeorder)
        {
            return vendorsOthersRepository.GetProducts_Results(Param,VID,servicetypesType,servicetypeloc,servicetypeorder);
        }

        public List<getservicetype_Result> Getservicetype_Result(string Param)
        {
            return vendorsOthersRepository.Getservicetype_Result(Param);
        }
    }
}
