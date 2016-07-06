﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaaAahwanam.Models;

namespace MaaAahwanam.Repository.db
{
    public class VendorsPhotographyRepository
    {
        readonly ApiContext _dbContext = new ApiContext();
        public List<dynamic> PhotographersList()
        {
            return _dbContext.VendorsPhotography.Join(_dbContext.Vendormaster, i => i.VendorMasterId, p => p.Id, (i, p) => new { p,i }).Join(_dbContext.VendorImage, w => w.p.Id, x => x.VendorId, (w, x) => new { w, x }).ToList<dynamic>();
        }

        public VendorsPhotography AddPhotography(VendorsPhotography vendorsPhotography)
        {
            _dbContext.VendorsPhotography.Add(vendorsPhotography);
            _dbContext.SaveChanges();
            return vendorsPhotography;
        }
    }
}
