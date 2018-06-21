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

        public int RemoveContest(long id)
        {
            int updatestatus;
            var record = _dbContext.ContestMaster.SingleOrDefault(i => i.ContentMasterID == id);
            ContestMaster contestMaster = new ContestMaster();
            contestMaster = record;
            contestMaster.Status = "Removed";
            contestMaster.UpdatedDate = date;
            _dbContext.Entry(record).CurrentValues.SetValues(contestMaster);
            updatestatus = _dbContext.SaveChanges();
            return updatestatus;
        }

        public int UpdateContestName(ContestMaster contestMaster)
        {
            int updatestatus;
            var record = _dbContext.ContestMaster.SingleOrDefault(i => i.ContentMasterID == contestMaster.ContentMasterID);
            //record.ContestName = contestMaster.ContestName;
            //record.UpdatedDate = date;
            contestMaster.UpdatedDate = date;
            contestMaster.CreatedDate = record.CreatedDate;
            contestMaster.Status = record.Status;
            _dbContext.Entry(record).CurrentValues.SetValues(contestMaster);
            updatestatus = _dbContext.SaveChanges();
            return updatestatus;
        }

        //Enter Contest
        public Contest EnterContest(Contest contest)
        {
            contest.CreatedDate = contest.UpdatedDate = date;
            contest.Status = "Pending";
            contest = _dbContext.Contest.Add(contest);
            _dbContext.SaveChanges();
            return contest;
        }

        public Contest Activationcontest(Contest cont1, string command)
        {
            var query =
                from ord in _dbContext.Contest
                where ord.ContestId == cont1.ContestId
                select ord;

            foreach (Contest ord in query)
            {
                ord.Status = command;
                // Insert any additional changes to column values.
            }
            _dbContext.SaveChanges();
            return cont1;
        }
        public List<Contest> GetAllEntries(long id)
        {
            return _dbContext.Contest.Where(m => m.ContentMasterID == id).ToList();
        }

        public int UpdateContestImage(long id,string image)
        {
            var record = from ord in _dbContext.Contest
                         where ord.ContestId == id
                         select ord;
            foreach (Contest ord in record)
            {
                ord.UploadedImage = image;
                // Insert any additional changes to column values.
            }
            int count =_dbContext.SaveChanges();
            //record.UploadedImage = contest.UploadedImage;
            //_dbContext.Entry(contest).CurrentValues.SetValues(record);
            return count;
        }

        //Vote Count
        public List<ContestVote> GetAllVotes(long id)
        {
            return _dbContext.ContestVote.Where(m => m.ContestId == id).ToList();
        }

        public ContestVote AddContestVote(ContestVote contestVote)
        {
            contestVote.VotedDate = contestVote.UpdatedDate = date;
            contestVote.Status = "Active";
            contestVote = _dbContext.ContestVote.Add(contestVote);
            _dbContext.SaveChanges();
            return contestVote;
        }

        public int RemoveContestVote(ContestVote contestVote)
        {
            var record = _dbContext.ContestVote.SingleOrDefault(i => i.ContestVoteID == contestVote.ContestVoteID);
            contestVote.UpdatedDate = date;
            contestVote.Status = "InActive";
            _dbContext.Entry(record).CurrentValues.SetValues(contestVote);
            int count = _dbContext.SaveChanges();
            return count;
        }

        public ContestVote ParticularVote(long id)
        {
            return _dbContext.ContestVote.Where(m => m.ContestVoteID == id).FirstOrDefault();
        }
    }
}
