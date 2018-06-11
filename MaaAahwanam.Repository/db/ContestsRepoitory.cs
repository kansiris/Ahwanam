using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaaAahwanam.Models;

namespace MaaAahwanam.Repository.db
{
    public class ContestsRepoitory
    {
        readonly ApiContext _dbContext = new ApiContext();
        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        DateTime date = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
        public ContestMaster AddNewContest(ContestMaster contestMaster)
        {
            contestMaster = _dbContext.ContestMaster.Add(contestMaster);
            _dbContext.SaveChanges();
            return contestMaster;
        }

        public List<ContestMaster> GetAllContests()
        {
            return _dbContext.ContestMaster.ToList();
        }

        public string RemoveContest(long id)
        {
            try
            {
                var list = _dbContext.ContestMaster.FirstOrDefault(m => m.ContentMasterID == id);
                _dbContext.ContestMaster.Remove(list);
                _dbContext.SaveChanges();
                return "Success";
            }
            catch
            {
                return "Failed";
            }
        }

        public int UpdateContestName(ContestMaster contestMaster)
        {
            int updatestatus;
            var record = _dbContext.ContestMaster.SingleOrDefault(i => i.ContentMasterID == contestMaster.ContentMasterID);
            record.ContestName = contestMaster.ContestName;
            record.UpdatedDate = date;
            _dbContext.Entry(record).CurrentValues.SetValues(contestMaster);
            updatestatus = _dbContext.SaveChanges();
            return updatestatus;
        }
    }
}
