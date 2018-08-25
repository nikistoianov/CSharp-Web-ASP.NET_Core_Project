namespace WCR.Services.Competition
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using WCR.Common.Competition.BindingModels;
    using WCR.Data;
    using WCR.Models;
    using WCR.Services.Competition.Interfaces;

    public class BetService : BaseEFService, IBetService
    {
        public BetService(WCRDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        { }

        public BetMatchBindingModel PrepareBetMatch(int matchId)
        {
            var match = this.DbContext.Matches
                .Where(x => x.Id == matchId)
                .Select(x => new BetMatchBindingModel()
                {
                    HomeTeam = x.FirstTeam.Name,
                    GuestTeam = x.SecondTeam.Name
                })
                .SingleOrDefault();

            return match;
        }

        public async Task<string> AddBetMatchAsync(int matchId, string userId, int homeTeamGoals, int guestTeamGoals)
        {
            var bet = new BetMatch()
            {
                FirstTeamGoals = homeTeamGoals,
                SecondTeamGoals = guestTeamGoals,
                MatchId = matchId,
                UserId = userId
            };

            await this.DbContext.BetsForMatch.AddAsync(bet);
            await this.DbContext.SaveChangesAsync();

            return null;
        }

        public BetMatchBindingModel GetBetMatch(int betId)
        {
            var bet = this.DbContext.BetsForMatch
                .Where(x => x.Id == betId)
                .Select(x => new BetMatchBindingModel()
                {
                    HomeTeam = x.Match.FirstTeam.Name,
                    GuestTeam = x.Match.SecondTeam.Name,
                    HomeTeamGoals = x.FirstTeamGoals,
                    GuestTeamGoals = x.SecondTeamGoals
                })
                .SingleOrDefault();

            return bet;
        }

        public async Task<string> EditBetMatchAsync(int betId, int homeTeamGoals, int guestTeamGoals)
        {
            var bet = await this.DbContext.BetsForMatch.FindAsync(betId);
            if (bet == null)
            {
                return "Bet not found.";
            }

            bet.FirstTeamGoals = homeTeamGoals;
            bet.SecondTeamGoals = guestTeamGoals;

            await this.DbContext.SaveChangesAsync();
            return null;
        }

        public BetMatch GetDbBetMatch(int id)
        {
            var bet = this.DbContext.BetsForMatch.Find(id);
            return bet;
        }

        public BetGroupBindingModel PrepareBetGroup(int groupId)
        {
            var group = this.DbContext.Groups
                .Where(x => x.Id == groupId)
                .Select(x => new BetGroupBindingModel()
                {
                    Teams = x.Teams
                        .Select(t => new BetTeamBindingModel()
                        {
                            Name = t.Name,
                            TeamId = t.Id
                        })
                        .ToArray()
                })
                .SingleOrDefault();

            return group;
        }

        public async Task<string> AddBetGroupAsync(string userId, BetGroupBindingModel model)
        {
            foreach (var team in model.Teams)
            {
                var bet = new BetPosition()
                {
                    Position = team.Position,
                    UserId = userId,
                    TeamId = team.TeamId
                };

                await this.DbContext.BetsForPosition.AddAsync(bet);
            }

            await this.DbContext.SaveChangesAsync();

            return null;
        }

        public BetGroupBindingModel GetBetGroup(int groupId, string userId)
        {
            var teams = this.DbContext.BetsForPosition
                .Where(x => x.Team.GroupId == groupId && x.UserId == userId)
                .Select(x => new BetTeamBindingModel()
                {
                    Position = x.Position,
                    Name = x.Team.Name,
                    TeamId = x.TeamId
                })
                .ToArray();

            var group = new BetGroupBindingModel()
            {
                Teams = teams
            };

            return group;
        }

        public async Task<string> EditBetGroupAsync(string userId, BetGroupBindingModel model)
        {
            foreach (var team in model.Teams)
            {
                var bet = this.DbContext.BetsForPosition
                    .Where(x => x.TeamId == team.TeamId && x.UserId == userId)
                    .SingleOrDefault();

                if (bet == null)
                {
                    return "Bet not found.";
                }

                bet.Position = team.Position;
            }

            await this.DbContext.SaveChangesAsync();

            return null;
        }

        public async Task<bool> IsBeggined(bool isGroup, int id)
        {
            if (isGroup)
            {
                var group = await this.DbContext.Groups.FindAsync(id);
                return group.Date <= DateTime.Now;
            }
            else
            {
                var match = await this.DbContext.Matches.FindAsync(id);
                return match.Date <= DateTime.Now;
            }
        }
    }
}
