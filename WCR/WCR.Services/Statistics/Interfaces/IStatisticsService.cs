namespace WCR.Services.Statistics.Interfaces
{
    using System.Collections.Generic;
    using WCR.Common.Statistics.ViewModels;

    public interface IStatisticsService
    {
        UserStatViewModel GetUserStatistics(string userId);

        ICollection<UserStatViewModel> GetAllUsersStatistics();
    }
}
