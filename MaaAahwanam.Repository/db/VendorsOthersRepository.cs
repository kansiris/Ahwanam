using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaaAahwanam.Models;
using System.Data.SqlClient;

namespace MaaAahwanam.Repository.db
{
    public class VendorsOthersRepository
    {
        readonly ApiContext _dbContext = new ApiContext();
        MaaAahwanamEntities maaAahwanamEntities = new MaaAahwanamEntities();
        public List<GetProducts_Result> GetProducts_Results(string parameters, int VID, string servicetypesType, string servicetypeloc, string servicetypeorder)
        {
            //maaAahwanamEntities.get
            return maaAahwanamEntities.GetProducts(parameters,VID, servicetypesType, servicetypeloc, servicetypeorder).ToList();
        }

        public List<ProductsDisplay_Result> ProductsDisplay(string parameters, int VID, string servicetypesType, string servicetypeloc, string servicetypeorder)
        {
            //maaAahwanamEntities.get
            return maaAahwanamEntities.ProductsDisplay(parameters, VID, servicetypesType, servicetypeloc, servicetypeorder).ToList();
        }

        //public List<sampleproc_Result> sampleproc(string VID)
        //{
        //    //maaAahwanamEntities.get
        //    return maaAahwanamEntities.sampleproc(VID).ToList();
        //}

        public List<SP_Deals_Result> GetSP_Deals_Result(string parameters, int VID, string servicetypesType, string servicetypeloc, string servicetypeorder)
        {
            //maaAahwanamEntities.get
            return maaAahwanamEntities.SP_Deals(parameters, VID, servicetypesType, servicetypeloc, servicetypeorder).ToList();
        }
        public List<sp_indexdeals_Result> gettopdeals(string type)
        {
            return maaAahwanamEntities.sp_indexdeals(type).ToList();
        }
        public List<getservicetype_Result> Getservicetype_Result(string parameters)
        {
            return maaAahwanamEntities.getservicetype(parameters).ToList();
        }
        public List<GetDealServiceType_Result> GetDealsservicetype_Result(string parameters)
        {
            return maaAahwanamEntities.GetDealServiceType(parameters).ToList();
        }

        //Product info page
        public GetProductsInfo_Result getProductsInfo(int vid,string servicetype,int Subvid)
        {
            var a= maaAahwanamEntities.GetProductsInfo(vid,servicetype, Subvid).FirstOrDefault();
            return a;
        }
        public SP_dealsinfo_Result getDealInfo(int vid, string servicetype, int Subvid,int did)
        {
            var a = maaAahwanamEntities.SP_dealsinfo(vid, servicetype, Subvid,did).FirstOrDefault();
            return a;
        }
    }
}
