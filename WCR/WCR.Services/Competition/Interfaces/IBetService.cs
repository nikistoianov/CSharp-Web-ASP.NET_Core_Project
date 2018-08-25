namespace WCR.Services.Competition.Interfaces
{
    using System.Threading.Tasks;
    using WCR.Common.Competition.BindingModels;
    using WCR.Models;

    public interface IBetService
    {
        BetMatchBindingModel PrepareBetMatch(int matchId);

        Task<string> AddBetMatchAsync(int matchId, string userId, int homeTeamGoals, int guestTeamGoals);

        BetMatchBindingModel GetBetMatch(int betId);

        Task<string> EditBetMatchAsync(int betId, int homeTeamGoals, int guestTeamGoals);

        BetMatch GetDbBetMatch(int id);

        BetGroupBindingModel PrepareBetGroup(int groupId);

        Task<string> AddBetGroupAsync(string userId, BetGroupBindingModel model);

        BetGroupBindingModel GetBetGroup(int groupId, string userId);

        Task<string> EditBetGroupAsync(string userId, BetGroupBindingModel model);

        Task<bool> IsBeggined(bool isGroup, int id);
    }
}
