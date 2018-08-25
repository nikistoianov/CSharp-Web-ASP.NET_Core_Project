namespace WCR.Common.Competition.ViewModels
{
    using System.Collections.Generic;

    public class GroupTeamViewModel
    {
        public GroupTeamViewModel()
        {
            this.Bets = new List<TeamBetViewModel>();
        }

        public string TeamName { get; set; }

        public string Position { get; set; }

        public IList<TeamBetViewModel> Bets { get; set; }
    }
}