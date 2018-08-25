namespace WCR.Services.Moderation.Interfaces
{
    using System.Threading.Tasks;
    using WCR.Common.Competition.BindingModels;
    using WCR.Common.Moderation.BindingModels;

    public interface IModerationService
    {
        Task<string> CreateGroup(GroupCreationBindingModel model);

        Task<TeamCreationBindingModel> PrepareTeamCreation();

        Task<string> CreateTeam(TeamCreationBindingModel model);

        Task<MatchCreationBindingModel> PrepareMatchCreation();

        Task<string> CreateMatch(MatchCreationBindingModel model);

        BetMatchBindingModel PrepareMatchScore(int matchId);

        Task<string> EditMatchScoreAsync(int matchId, int homeTeamGoals, int guestTeamGoals);

        BetGroupBindingModel PrepareGroupScore(int groupId);

        Task<string> EditGroupScoreAsync(int groupId, BetGroupBindingModel model);
    }
}
