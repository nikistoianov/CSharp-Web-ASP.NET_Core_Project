namespace WCR.Models
{
    using System.ComponentModel.DataAnnotations;

    public class BetPosition
    {
        public int TeamId { get; set; }
        public Team Team { get; set; }

        public int Position { get; set; }

        [Required]
        public string UserId { get; set; }
        public User User { get; set; }
    }
}
