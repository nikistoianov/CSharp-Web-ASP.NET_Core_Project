namespace WCR.Services.Competition.Interfaces
{
    using System.Collections.Generic;
    using WCR.Common.Competition.ViewModels;

    public interface IRoundService
    {
        IList<MatchViewModel> GetMatches(int roundIndex);

        void ArrangeScoreBets(ICollection<MatchViewModel> matches, ICollection<UserDetailsViewModel> users, string currentUserId, bool isAdmin);

        string GetRoundTitle(int id);

        ICollection<MidResultViewModel> GetRoundResults(IList<MatchViewModel> matches, int usersCount);

        ICollection<MidResultViewModel> GetRoundResults(int roundIndex);

        IList<MidResultViewModel> GetBonusResults(ICollection<MidResultViewModel> roundResults);

        IList<TotalResultViewModel> GetTotalResults(ICollection<MidResultViewModel> roundResults, IList<MidResultViewModel> bonusResults);

        IList<TotalResultViewModel> JoinTotalResults(IList<TotalResultViewModel> prevResults, IList<TotalResultViewModel> totalResults);

        void AddCurrentTimeDelimiter(ref IList<MatchViewModel> matches);
    }
}
