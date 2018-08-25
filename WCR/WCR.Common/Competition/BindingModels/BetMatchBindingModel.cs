namespace WCR.Common.Competition.BindingModels
{
    using System.ComponentModel.DataAnnotations;

    public class BetMatchBindingModel
    {
        public string HomeTeam { get; set; }

        public string GuestTeam { get; set; }

        [Required]
        [Range(0, 99)]
        public int GuestTeamGoals { get; set; }

        [Required]
        [Range(0, 99)]
        public int HomeTeamGoals { get; set; }
    }
}
