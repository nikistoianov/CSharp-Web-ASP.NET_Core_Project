namespace WCR.Services.Administration.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using WCR.Common.Administration.ViewModels;

    public interface IAdminService
    {
        IEnumerable<TeamViewModel> GetTeams();

        Task<string> DeleteTeamAsync(int id);

        IEnumerable<MatchViewModel> GetMatches();

        Task<string> DeleteMatchAsync(int id);
    }
}
