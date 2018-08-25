namespace WCR.Services.Administration
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using WCR.Common.Administration.ViewModels;
    using WCR.Data;
    using WCR.Services.Administration.Interfaces;

    public class AdminService : BaseEFService, IAdminService
    {
        public AdminService(WCRDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        { }

        public IEnumerable<TeamViewModel> GetTeams()
        {
            var teams = DbContext.Teams
                .Select(x => new TeamViewModel()
                {
                    Id = x.Id,
                    Name = x.Name,
                    GroupName = x.Group.Name
                })
                .OrderBy(x => x.GroupName)
                .ToList();
            return teams;
        }

        public async Task<string> DeleteTeamAsync(int id)
        {
            var team = await DbContext.Teams.FindAsync(id);
            if (team == null)
            {
                return $"Team with id: {id} not found!";
            }

            DbContext.Teams.Remove(team);
            await DbContext.SaveChangesAsync();

            return null;
        }

        public IEnumerable<MatchViewModel> GetMatches()
        {
            var matches = DbContext.Matches
                .Select(x => new MatchViewModel()
                {
                    Id = x.Id,
                    HomeTeamId = x.FirstTeamId,
                    HomeTeam = x.FirstTeam.Name,
                    GuestTeamId = x.SecondTeamId,
                    GuestTeam = x.SecondTeam.Name,
                    Score = x.FirstTeamGoals.ToString() + ":" + x.SecondTeamGoals.ToString() ?? "no score",
                    Date = x.Date
                })
                .OrderBy(x => x.Date)
                .ToList();
            return matches;
        }

        public async Task<string> DeleteMatchAsync(int id)
        {
            var match = await DbContext.Matches.FindAsync(id);
            if (match == null)
            {
                return $"Match with id: {id} not found!";
            }

            DbContext.Matches.Remove(match);
            await DbContext.SaveChangesAsync();

            return null;
        }
    }
}
