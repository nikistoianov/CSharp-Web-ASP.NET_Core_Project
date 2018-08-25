namespace WCR.Common.Moderation.BindingModels
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class GroupCreationBindingModel
    {
        [Required]
        [Display(Name = "Group name")]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Start date of the group")]
        public DateTime Date { get; set; }
    }
}
