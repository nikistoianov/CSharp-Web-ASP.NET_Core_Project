namespace WCR.Common.Competition.ViewModels
{
    using System.Collections.Generic;

    public class GroupsViewModel
    {
        public GroupsViewModel()
        {
            this.Users = new List<UserDetailsViewModel>();
            this.Groups = new List<GroupViewModel>();
            this.RoundPoints = new List<MidResultViewModel>();
            this.BonusPoints = new List<MidResultViewModel>();
            this.TotalPoints = new List<TotalResultViewModel>();
        }

        public ICollection<UserDetailsViewModel> Users { get; set; }
        public ICollection<GroupViewModel> Groups { get; set; }
        public ICollection<MidResultViewModel> RoundPoints { get; set; }
        public ICollection<MidResultViewModel> BonusPoints { get; set; }
        public ICollection<TotalResultViewModel> TotalPoints { get; set; }
    }
}
