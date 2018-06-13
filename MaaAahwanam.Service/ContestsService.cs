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
    public class ContestsService
    {
        ContestsRepoitory contestsRepoitory = new ContestsRepoitory();

        public ContestMaster AddNewContest(ContestMaster contestMaster)
        {
            return contestsRepoitory.AddNewContest(contestMaster);
        }

        public List<ContestMaster> GetAllContests()
        {
            return contestsRepoitory.GetAllContests();
        }

        public int RemoveContest(long id)
        {
            return contestsRepoitory.RemoveContest(id);
        }

        public Contest Activationcontest(Contest cont1,string command)
        {

            return contestsRepoitory.Activationcontest(cont1,command);
        }

        public int UpdateContestName(ContestMaster contestMaster)
        {
            return contestsRepoitory.UpdateContestName(contestMaster);
        }

        //Enter Contest
        public Contest EnterContest(Contest contest)
        {
            return contestsRepoitory.EnterContest(contest);
        }

        public List<Contest> GetAllEntries(long id)
        {
            return contestsRepoitory.GetAllEntries(id);
        }

        //Vote Count
        public List<ContestVote> GetAllVotes(long id)
        {
            return contestsRepoitory.GetAllVotes(id);
        }

        public ContestVote AddContestVote(ContestVote contestVote)
        {
            return contestsRepoitory.AddContestVote(contestVote);
        }

        public int RemoveContestVote(ContestVote contestVote)
        {
            return contestsRepoitory.RemoveContestVote(contestVote);
        }

        public ContestVote ParticularVote(long id)
        {
            return contestsRepoitory.ParticularVote(id);
        }
    }
}
