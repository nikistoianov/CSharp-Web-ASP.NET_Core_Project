namespace WCR.Services.Moderation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using WCR.Common.Competition.BindingModels;
    using WCR.Common.Moderation.BindingModels;
    using WCR.Data;
    using WCR.Models;
    using WCR.Services.Moderation.Interfaces;

    public class ModerationService : BaseEFService, IModerationService
    {
        public ModerationService(WCRDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        { }

        public async Task<string> CreateGroup(GroupCreationBindingModel model)
        {
            var existingGroupName = DbContext.Groups.Any(x => x.Name == model.Name);
            if (existingGroupName)
            {
                return $"Group with name {model.Name} exists.";
            }

            var group = Mapper.Map<Group>(model);

            await DbContext.Groups.AddAsync(group);
            await DbContext.SaveChangesAsync();

            return null;
        }

        public Task<TeamCreationBindingModel> PrepareTeamCreation()
        {
            var groups = DbContext.Groups.ToArray();
            var groupList = new List<SelectListItem>();
            foreach (var group in groups)
            {
                var item = new SelectListItem { Value = group.Id.ToString(), Text = group.Name };
                groupList.Add(item);
            }

            var result = new TeamCreationBindingModel() { Groups = groupList };
            return Task.FromResult(result);
        }

        public async Task<string> CreateTeam(TeamCreationBindingModel model)
        {
            var existingTeam = DbContext.Teams.Any(x => x.Name == model.Name);
            if (existingTeam)
            {
                return $"Team with name {model.Name} exists.";
            }

            var team = Mapper.Map<Team>(model);

            await DbContext.Teams.AddAsync(team);
            await DbContext.SaveChangesAsync();

            return null;
        }

        public Task<MatchCreationBindingModel> PrepareMatchCreation()
        {
            var teams = DbContext.Teams.ToArray();
            var teamList = new List<SelectListItem>();
            foreach (var team in teams)
            {
                var item = new SelectListItem { Value = team.Id.ToString(), Text = team.Name };
                teamList.Add(item);
            }

            var result = new MatchCreationBindingModel() { Teams = teamList, Date = DateTime.Now, RoundIndex = 1 };
            return Task.FromResult(result);
        }

        public async Task<string> CreateMatch(MatchCreationBindingModel model)
        {
            var homeTeam = await DbContext.Teams.FindAsync(model.FirstTeamId);
            if (homeTeam == null)
            {
                return "Home team not found.";
            }

            var guestTeam = await DbContext.Teams.FindAsync(model.SecondTeamId);
            if (guestTeam == null)
            {
                return "Guest team not found.";
            }

            var match = Mapper.Map<Match>(model);

            await DbContext.Matches.AddAsync(match);
            await DbContext.SaveChangesAsync();

            return null;
        }

        public BetMatchBindingModel PrepareMatchScore(int matchId)
        {
            var model = this.DbContext.Matches
                .Where(x => x.Id == matchId)
                .Select(x => new BetMatchBindingModel()
                {
                    HomeTeam = x.FirstTeam.Name,
                    GuestTeam = x.SecondTeam.Name,
                    HomeTeamGoals = x.FirstTeamGoals ?? 0,
                    GuestTeamGoals = x.SecondTeamGoals ?? 0
                })
                .SingleOrDefault();

            return model;
        }

        public async Task<string> EditMatchScoreAsync(int matchId, int homeTeamGoals, int guestTeamGoals)
        {
            var match = await this.DbContext.Matches.FindAsync(matchId);
            if (match == null)
            {
                return "Match not found.";
            }

            match.FirstTeamGoals = homeTeamGoals;
            match.SecondTeamGoals = guestTeamGoals;

            await this.DbContext.SaveChangesAsync();
            return null;
        }

        public BetGroupBindingModel PrepareGroupScore(int groupId)
        {
            var model = this.DbContext.Groups
                .Where(x => x.Id == groupId)
                .Select(x => new BetGroupBindingModel()
                {
                    Teams = x.Teams
                        .Select(t => new BetTeamBindingModel()
                        {
                            Name = t.Name,
                            TeamId = t.Id,
                            Position = t.GroupPosition == null ? 0 : t.GroupPosition.Value
                        })
                        .ToArray()
                })
                .SingleOrDefault();

            return model;
        }

        public async Task<string> EditGroupScoreAsync(int groupId, BetGroupBindingModel model)
        {
            foreach (var team in model.Teams)
            {
                var dbTeam = await this.DbContext.Teams.FindAsync(team.TeamId);
                if (dbTeam == null)
                {
                    return "Team not foud.";
                }
                dbTeam.GroupPosition = team.Position;
            }

            await this.DbContext.SaveChangesAsync();
            return null;
        }
    }
}
