namespace WCR.Common.Moderation.BindingModels
{
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class TeamCreationBindingModel
    {
        public TeamCreationBindingModel()
        {
            this.Groups = new List<SelectListItem>();
        }

        [Required]
        [Display(Name = "Team name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "You have to specify a group.")]
        [Display(Name = "Group")]
        public int GroupId { get; set; }

        [BindNever]
        public IEnumerable<SelectListItem> Groups { get; set; }
    }
}
