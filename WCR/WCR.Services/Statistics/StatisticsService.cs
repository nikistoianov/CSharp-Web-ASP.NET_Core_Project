namespace WCR.Services.Statistics
{
    using AutoMapper;
    using System.Collections.Generic;
    using System.Linq;
    using WCR.Common.Statistics.ViewModels;
    using WCR.Data;
    using WCR.Services.Statistics.Interfaces;

    public class StatisticsService : BaseEFService, IStatisticsService
    {
        public StatisticsService(WCRDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        { }

        public UserStatViewModel GetUserStatistics(string userId)
        {
            var user = this.DbContext.Users
                .Where(x => x.Id == userId)
                .Select(x => new UserStatViewModel()
                {
                    ShortName = x.ShortName,
                    Email = x.Email,
                    BetsMatch = x.BetsForMatches.Count,
                    BetsTeam = x.BetsForPosition.Count
                })
                .SingleOrDefault();

            return user;
        }

        public ICollection<UserStatViewModel> GetAllUsersStatistics()
        {
            var users = this.DbContext.Users
                .Select(x => new UserStatViewModel()
                {
                    ShortName = x.ShortName,
                    Email = x.Email,
                    BetsMatch = x.BetsForMatches.Count,
                    BetsTeam = x.BetsForPosition.Count
                })
                .OrderBy(x => x.ShortName)
                .ToArray();

            return users;
        }

    }
}
