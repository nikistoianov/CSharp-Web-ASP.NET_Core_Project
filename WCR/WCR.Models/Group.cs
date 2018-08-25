namespace WCR.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Group
    {
        public Group()
        {
            this.Teams = new List<Team>();
        }

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public DateTime Date { get; set; }

        public ICollection<Team> Teams { get; set; }
    }
}
