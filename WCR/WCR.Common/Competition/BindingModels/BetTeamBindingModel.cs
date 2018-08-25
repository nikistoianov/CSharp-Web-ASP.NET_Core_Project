namespace WCR.Common.Competition.BindingModels
{
    using System.ComponentModel.DataAnnotations;

    public class BetTeamBindingModel
    {
        public string Name { get; set; }

        [Required]
        [Range(1, 4)]
        public int Position { get; set; }

        public int TeamId { get; set; }
    }
}