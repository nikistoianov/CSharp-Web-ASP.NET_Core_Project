namespace WCR.Models
{
    using System;
    using System.Collections.Generic;

    public class Match
    {
        public Match()
        {
            this.Bets = new List<BetMatch>();
        }

        public int Id { get; set; }

        public int FirstTeamId { get; set; }
        public Team FirstTeam { get; set; }

        public int SecondTeamId { get; set; }
        public Team SecondTeam { get; set; }

        public int? FirstTeamGoals { get; set; }

        public int? SecondTeamGoals { get; set; }

        public int RoundIndex { get; set; }

        public DateTime Date { get; set; }

        public ICollection<BetMatch> Bets { get; set; }
    }
}
