namespace WCR.Models
{
    using System.ComponentModel.DataAnnotations;

    public class BetMatch
    {
        public int Id { get; set; }

        public int FirstTeamGoals { get; set; }

        public int SecondTeamGoals { get; set; }

        public int MatchId { get; set; }
        public Match Match { get; set; }

        [Required]
        public string UserId { get; set; }
        public User User { get; set; }
    }
}
