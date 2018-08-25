namespace WCR.Common.Competition.ViewModels
{
    using System;
    using System.Collections.Generic;

    public class MatchViewModel
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public string DelimiterText { get; set; }

        public string FirstTeam { get; set; }

        public string SecondTeam { get; set; }

        public string Score { get; set; }

        public IList<ScoreBetViewModel> ScoreBets { get; set; }
    }
}
