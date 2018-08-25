namespace WCR.Common.Moderation.BindingModels
{
    using Microsoft.AspNetCore.Mvc.Rendering;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using WCR.Common.Attributes;

    public class MatchCreationBindingModel
    {
        public MatchCreationBindingModel()
        {
            this.Teams = new List<SelectListItem>();
        }

        [Required]
        [Range(1, 4)]
        [Display(Name = "Round Index")]
        public int RoundIndex { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Start date of the match")]
        public DateTime Date { get; set; }

        [Required]
        [Display(Name = "Home team")]
        public int FirstTeamId { get; set; }

        [Required]
        [Display(Name = "Guest team")]
        [NotEqualTo("FirstTeamId", ErrorMessage = "'Guest team' and 'Home team' can not be the same.")]
        public int SecondTeamId { get; set; }

        public IEnumerable<SelectListItem> Teams { get; set; }
    }
}
