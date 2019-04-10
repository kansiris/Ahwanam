using MaaAahwanam.Models;
using MaaAahwanam.Repository.db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaaAahwanam.Repository;

namespace MaaAahwanam.Service
{
    public class CeremonyServices
    {
        CeremonyRepository ceremonyrepo = new CeremonyRepository();

         public List<Ceremony> Getall()
        {
            return ceremonyrepo.Getall();
        }
        public List<Ceremony> Getallbasedtype(int type)
        {
            return ceremonyrepo.Getalleventtype(type);
        }
        public Ceremony Getceremony(string pagename)
        {
            return ceremonyrepo.Getceremonydetails(pagename);
        }
        public List<CeremonyCategory> getceremonydetails(long id)
        {
            return ceremonyrepo.getceremonycategory(id);
        }

        public List<Ceremonydetails_Result> Getdetails(string ceremony)
        {
            return ceremonyrepo.GetDetails(ceremony);
        }

        public List<Ceremonydetails_id_Result> Getceremonydetails(long ceremonyid)
        {
            return ceremonyrepo.Getceremonydetails(ceremonyid);
        }

        public List<Getwedding_Result> GetVendorDetails(string ceremony)
        {
            return ceremonyrepo.GetVendorDetails(ceremony);
        }
    }
}
