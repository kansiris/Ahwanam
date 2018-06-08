﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaaAahwanam.Models;

namespace MaaAahwanam.Repository.db
{
    public class VendormasterRepository
    {
        readonly ApiContext _dbContext = new ApiContext();
        public List<Vendormaster> VendormasterList()
        {
            return _dbContext.Vendormaster.ToList();
        }

        public Vendormaster AddVendorMaster(Vendormaster vendorMaster)
        {
            _dbContext.Vendormaster.Add(vendorMaster);
            _dbContext.SaveChanges();
            return vendorMaster;
        }

        public Vendormaster GetVendor(long id)
        {
            return _dbContext.Vendormaster.Where(m => m.Id == id).FirstOrDefault();
        }

        public Vendormaster UpdateVendorMaster(Vendormaster vendorMaster, long id)
        {
            var GetMasterRecord = _dbContext.Vendormaster.SingleOrDefault(m => m.Id == id);
            vendorMaster.Id = GetMasterRecord.Id;
            vendorMaster.ContactPerson = GetMasterRecord.ContactPerson;
            vendorMaster.Status = GetMasterRecord.Status;
            vendorMaster.UpdatedDate = GetMasterRecord.UpdatedDate;
            if (GetMasterRecord.ServicType.Split(',').Contains(vendorMaster.ServicType) == false)
                vendorMaster.ServicType = string.Join(",", (GetMasterRecord.ServicType + "," + vendorMaster.ServicType).Split(',').Distinct()).TrimEnd(',');
            _dbContext.Entry(GetMasterRecord).CurrentValues.SetValues(vendorMaster);
            _dbContext.SaveChanges();
            return vendorMaster;
        }

        public int checkemail(string emailid)
        {
            int i = _dbContext.Vendormaster.Where(m => m.EmailId == emailid).Count();
            return i;
        }

        public Vendormaster GetVendorServiceType(long id)
        {
            var list = _dbContext.UserLogin.Where(m => m.UserLoginId == id).FirstOrDefault();
            return _dbContext.Vendormaster.Where(m => m.EmailId == list.UserName).FirstOrDefault();
        }

        public Vendormaster GetVendorByEmail(string emailid)
        {
            return _dbContext.Vendormaster.Where(m => m.EmailId == emailid).FirstOrDefault();
        }

        //public List<> amenities(string type,long id)
        //{

        //}

        public Vendormaster UpdateVendorStorefront(Vendormaster vendorMaster, long id)
        {
            var GetMasterRecord = _dbContext.Vendormaster.SingleOrDefault(m => m.Id == id);
            vendorMaster.Id = GetMasterRecord.Id;
            vendorMaster.ContactPerson = GetMasterRecord.ContactPerson;
            vendorMaster.Status = GetMasterRecord.Status;
            vendorMaster.UpdatedDate = GetMasterRecord.UpdatedDate;
            if (GetMasterRecord.ServicType.Split(',').Contains(vendorMaster.ServicType) == false)
                vendorMaster.ServicType = GetMasterRecord.ServicType + "," + vendorMaster.ServicType;//string.Join(",", (GetMasterRecord.ServicType + "," + vendorMaster.ServicType).Split(',').Distinct()).TrimEnd(',');
            else
                vendorMaster.ServicType = GetMasterRecord.ServicType;
            _dbContext.Entry(GetMasterRecord).CurrentValues.SetValues(vendorMaster);
            _dbContext.SaveChanges();
            return vendorMaster;
        }

        public Vendormaster UpdateVendorDetails(Vendormaster vendorMaster, long id)
        {
            var GetMasterRecord = _dbContext.Vendormaster.SingleOrDefault(m => m.Id == id);
            _dbContext.Entry(GetMasterRecord).CurrentValues.SetValues(vendorMaster);
            _dbContext.SaveChanges();
            return vendorMaster;
        }
    }
}
