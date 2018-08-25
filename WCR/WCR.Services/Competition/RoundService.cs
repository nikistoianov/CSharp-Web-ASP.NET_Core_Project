namespace WCR.Services.Competition
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using WCR.Common.Competition.ViewModels;
    using WCR.Common.Constants;
    using WCR.Data;
    using WCR.Models;
    using WCR.Services.Competition.Interfaces;

    public class RoundService : BaseEFService, IRoundService
    {
        public RoundService(WCRDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        { }

        public IList<MatchViewModel> GetMatches(int roundIndex)
        {
            var matches = this.DbContext.Matches
                .Where(x => x.RoundIndex == roundIndex)
                .Select(x => new MatchViewModel()
                {
                    Id = x.Id,
                    Date = x.Date,
                    FirstTeam = x.FirstTeam.Name,
                    SecondTeam = x.SecondTeam.Name,
                    Score = x.FirstTeamGoals.ToString() + " : " + x.SecondTeamGoals ?? Constants.NO_SCORE,
                    ScoreBets = x.Bets.Select(b => new ScoreBetViewModel()
                    {
                        Id = b.Id,
                        Score = b.FirstTeamGoals.ToString() + " : " + b.SecondTeamGoals.ToString(),
                        User = b.User.ShortName,
                        Points = CalculatePoints(b, b.Match)
                    }).ToArray()
                })
                .OrderBy(x => x.Date)
                .ToList();

            return matches;
        }

        private int CalculatePoints(BetMatch bet, Match match)
        {
            if (match.FirstTeamGoals == null || match.SecondTeamGoals == null)
            {
                return 0;
            }
            else if (match.FirstTeamGoals == bet.FirstTeamGoals && match.SecondTeamGoals == bet.SecondTeamGoals)
            {
                return Constants.POINTS_MATCH_BIG_SCORE;
            }
            else if ((match.FirstTeamGoals == match.SecondTeamGoals && bet.FirstTeamGoals == bet.SecondTeamGoals)
                  || (match.FirstTeamGoals > match.SecondTeamGoals && bet.FirstTeamGoals > bet.SecondTeamGoals)
                  || (match.FirstTeamGoals < match.SecondTeamGoals && bet.FirstTeamGoals < bet.SecondTeamGoals))
            {
                return Constants.POINTS_MATCH_SMALL_SCORE;
            }
            else
            {
                return 0;
            }
        }

        public void ArrangeScoreBets(ICollection<MatchViewModel> matches, ICollection<UserDetailsViewModel> users, string currentUserId, bool isAdmin)
        {
            foreach (var match in matches)
            {
                var newScoreBets = new List<ScoreBetViewModel>();
                foreach (var user in users)
                {
                    var foundScoreBet = match.ScoreBets.FirstOrDefault(x => x.User == user.ShortName);
                    if (foundScoreBet == null)
                    {
                        foundScoreBet = new ScoreBetViewModel() { User = user.ShortName, Score = Constants.NO_SCORE };
                    }

                    var isCurrentUser = currentUserId != null && currentUserId == user.Id;
                    var isStarted = match.Date < DateTime.Now;

                    foundScoreBet.Hidden = !isAdmin && !isStarted && !isCurrentUser;
                    foundScoreBet.Current = (isCurrentUser && !isStarted) || (isAdmin && foundScoreBet.Score != Constants.NO_SCORE);
                    foundScoreBet.MatchId = match.Id;
                    foundScoreBet.ClassName = GetScoreClass(foundScoreBet.Points);

                    if (foundScoreBet.Points > 0)
                    {
                        foundScoreBet.Score += $" ({foundScoreBet.Points})";
                    }

                    newScoreBets.Add(foundScoreBet);
                }
                match.ScoreBets = newScoreBets;
            }
        }

        private string GetScoreClass(int points)
        {
            switch (points)
            {
                case 0:
                    return Constants.CLASS_NO_SCORE;
                case Constants.POINTS_MATCH_SMALL_SCORE:
                    return Constants.CLASS_SMALL_SCORE;
                case Constants.POINTS_MATCH_BIG_SCORE:
                    return Constants.CLASS_BIG_SCORE;
                default:
                    return Constants.CLASS_NO_SCORE;
            }
        }

        public string GetRoundTitle(int id)
        {
            switch (id)
            {
                case 1:
                    return "Round I";
                case 2:
                    return "Round II";
                case 3:
                    return "Round III";
                default:
                    return "Final Round";
            }
        }

        public ICollection<MidResultViewModel> GetRoundResults(int roundIndex)
        {
            var result = this.DbContext.Users
                .OrderBy(x => x.ShortName)
                .Select(x => new MidResultViewModel()
                {
                    Points = x.BetsForMatches
                    .Where(b => b.Match.RoundIndex == roundIndex)
                    .Sum(b => CalculatePoints(b, b.Match))
                })
                .ToArray();

            return result;
        }

        public ICollection<MidResultViewModel> GetRoundResults(IList<MatchViewModel> matches, int usersCount)
        {
            var result = new MidResultViewModel[usersCount];
            for (int i = 0; i < usersCount; i++)
            {
                result[i] = new MidResultViewModel();
            }

            foreach (var match in matches)
            {
                for (int i = 0; i < match.ScoreBets.Count; i++)
                {
                    result[i].Points += match.ScoreBets[i].Points;
                }
            }
            return result;
        }

        public IList<MidResultViewModel> GetBonusResults(ICollection<MidResultViewModel> roundResults)
        {
            var max = roundResults.Count == 0 ? 0 : roundResults.Max(x => x.Points);
            var result = roundResults
                .Select(x => new MidResultViewModel()
                {
                    Points = (x.Points == max) && max > 0 ? Constants.POINTS_BONUS : 0
                })
                .ToArray();

            return result;
        }

        public IList<TotalResultViewModel> GetTotalResults(ICollection<MidResultViewModel> roundResults, IList<MidResultViewModel> bonusResults)
        {
            var result = roundResults
                .Select((x, i) => new TotalResultViewModel()
                {
                    Points = x.Points + bonusResults[i].Points
                })
                .ToArray();

            return result;
        }

        public IList<TotalResultViewModel> JoinTotalResults(IList<TotalResultViewModel> prevResults, IList<TotalResultViewModel> totalResults)
        {
            var result = prevResults
                .Select((x, i) => new TotalResultViewModel()
                {
                    Points = x.Points + totalResults[i].Points
                })
                .ToArray();

            return result;
        }

        public void AddCurrentTimeDelimiter(ref IList<MatchViewModel> matches)
        {
            if (matches.Count > 0 && matches.Last().Date > DateTime.Now)
            {
                matches.Insert(0, new MatchViewModel() { Date = DateTime.Now, DelimiterText = "upcomming matches" });
                matches = matches.OrderBy(x => x.Date).ToList();
            }
        }
    }
}
