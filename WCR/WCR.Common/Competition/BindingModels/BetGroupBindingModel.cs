namespace WCR.Common.Competition.BindingModels
{
    using System.Collections.Generic;

    public class BetGroupBindingModel
    {
        public BetGroupBindingModel()
        {
            this.Teams = new List<BetTeamBindingModel>();
        }

        public ICollection<BetTeamBindingModel> Teams { get; set; }
    }
}
