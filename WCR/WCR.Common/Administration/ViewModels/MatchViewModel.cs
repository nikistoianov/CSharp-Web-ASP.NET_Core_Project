namespace WCR.Common.Administration.ViewModels
{
    using System;

    public class MatchViewModel
    {
        public int Id { get; set; }

        public int HomeTeamId { get; set; }
        public string HomeTeam { get; set; }

        public int GuestTeamId { get; set; }
        public string GuestTeam { get; set; }

        public string Score { get; set; }

        public DateTime Date { get; set; }
    }
}
