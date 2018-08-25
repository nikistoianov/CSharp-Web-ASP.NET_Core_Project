namespace WCR.Common.Competition.ViewModels
{
    public class TeamBetViewModel
    {
        public int GroupId { get; set; }

        public string Score { get; set; }

        public string UserId { get; set; }

        public int Points { get; set; }

        public bool Hidden { get; set; }

        public bool Current { get; set; }

        public string ClassName { get; set; }
    }
}