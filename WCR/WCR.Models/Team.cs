namespace WCR.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Team
    {
        public Team()
        {
            this.HomeMatches = new List<Match>();
            this.GuestMatches = new List<Match>();
            this.BetsForPosition = new List<BetPosition>();
        }

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public int GroupId { get; set; }
        public Group Group { get; set; }

        public int? GroupPosition { get; set; }

        public ICollection<Match> HomeMatches { get; set; }
        public ICollection<Match> GuestMatches { get; set; }

        public ICollection<BetPosition> BetsForPosition { get; set; }
    }
}
