namespace WCR.Services.Competition.Interfaces
{
    using System.Collections.Generic;
    using WCR.Common.Competition.ViewModels;

    public interface IGroupService
    {
        IList<GroupViewModel> GetGroups();

        void ArrangeTeamBets(ICollection<GroupViewModel> groups, ICollection<UserDetailsViewModel> users, string currentUserId, bool isAdmin);

        ICollection<MidResultViewModel> GetRoundResults(IList<GroupViewModel> groups, int usersCount);

        ICollection<MidResultViewModel> GetRoundResults();
    }
}
