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

        public List<ProductsDisplay_Result> ProductsDisplay(string Param, int VID, string servicetypesType, string servicetypeloc, string servicetypeorder)
        {
            return vendorsOthersRepository.ProductsDisplay(Param, VID, servicetypesType, servicetypeloc, servicetypeorder);
        }
        //public List<sampleproc_Result> sampleproc(string VID)
        //{
        //    return vendorsOthersRepository.sampleproc( VID);
        //}

        public List<SP_Deals_Result> GetSP_Deals_Result(string Param, int VID, string servicetypesType, string servicetypeloc, string servicetypeorder)
        {
            return vendorsOthersRepository.GetSP_Deals_Result(Param, VID, servicetypesType, servicetypeloc, servicetypeorder);
        }
        public List<sp_indexdeals_Result> gettopdealsservice(string type)
        {
            return vendorsOthersRepository.gettopdeals(type);
        }
        public List<getservicetype_Result> Getservicetype_Result(string Param)
        {
            return vendorsOthersRepository.Getservicetype_Result(Param);
        }
        public List<GetDealServiceType_Result> GetDealsservicetype_Result(string Param)
        {
            return vendorsOthersRepository.GetDealsservicetype_Result(Param);
        }
    }
}
