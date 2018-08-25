namespace WCR.Common.Competition.ViewModels
{
    public class ScoreBetViewModel
    {
        public int Id { get; set; }

        public int MatchId { get; set; }

        public string Score { get; set; }

        public string User { get; set; }

        public int Points { get; set; }

        public bool Hidden { get; set; }

        public bool Current { get; set; }

        public string ClassName { get; set; }
    }
}