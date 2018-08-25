namespace WCR.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Identity;

    public class User : IdentityUser
    {
        public User()
        {
            this.BetsForMatches = new List<BetMatch>();
            this.BetsForPosition = new List<BetPosition>();
        }

        [Required]
        [StringLength(6, MinimumLength = 3)]
        public string ShortName { get; set; }

        public ICollection<BetMatch> BetsForMatches { get; set; }
        public ICollection<BetPosition> BetsForPosition { get; set; }
    }
}
