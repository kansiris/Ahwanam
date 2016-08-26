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
        public List<getservicetype_Result> Getservicetype_Result(string parameters)
        {
            return maaAahwanamEntities.getservicetype(parameters).ToList();
        }

        //Product info page
        public GetProductsInfo_Result getProductsInfo(int vid,string servicetype)
        {
            var a= maaAahwanamEntities.GetProductsInfo(vid,servicetype).FirstOrDefault();
            return a;
        }
    }
}
