namespace WCR.Web.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using WCR.Common.Competition.ViewModels;
    using WCR.Common.Constants;
    using WCR.Models;
    using WCR.Services.Competition.Interfaces;

    public class CompetitionController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly IMapper mapper;
        private readonly IRoundService roundService;
        private readonly IGroupService groupService;

        public CompetitionController(
            UserManager<User> userManager,
            IMapper mapper,
            IRoundService roundService,
            IGroupService groupService)
        {
            this.userManager = userManager;
            this.mapper = mapper;
            this.roundService = roundService;
            this.groupService = groupService;
        }

        public IActionResult Groups()
        {
            var users = userManager.Users
                .OrderBy(x => x.ShortName)
                .ToList();

            var groups = groupService.GetGroups();
            var mappedUsers = mapper.Map<List<UserDetailsViewModel>>(users);
            var currentUserId = userManager.GetUserId(User);
            var isAdmin = User.IsInRole(Constants.ROLE_ADMIN);

            groupService.ArrangeTeamBets(groups, mappedUsers, currentUserId, isAdmin);

            var roundPoints = groupService.GetRoundResults(groups, users.Count);
            var bonusPoints = roundService.GetBonusResults(roundPoints);
            var totalPoints = roundService.GetTotalResults(roundPoints, bonusPoints);

            var model = new GroupsViewModel()
            {
                Users = mappedUsers,
                Groups = groups,
                RoundPoints = roundPoints,
                BonusPoints = bonusPoints,
                TotalPoints = totalPoints
            };


            return View(model);
        }

        public IActionResult Rounds(int id)
        {
            var users = userManager.Users
                .OrderBy(x => x.ShortName)
                .ToList();

            var matches = roundService.GetMatches(id);
            var mappedUsers = mapper.Map<List<UserDetailsViewModel>>(users);
            var currentUserId = userManager.GetUserId(User);
            var isAdmin = User.IsInRole(Constants.ROLE_ADMIN);

            roundService.ArrangeScoreBets(matches, mappedUsers, currentUserId, isAdmin);

            var roundPoints = roundService.GetRoundResults(matches, users.Count);
            var bonusPoints = roundService.GetBonusResults(roundPoints);
            var totalPoints = roundService.GetTotalResults(roundPoints, bonusPoints);

            if (id > 1)
            {
                for (int i = 1; i < id; i++)
                {
                    var prevRoundPoints = roundService.GetRoundResults(i);
                    var prevBonusPoints = roundService.GetBonusResults(prevRoundPoints);
                    var prevTotalPoints = roundService.GetTotalResults(prevRoundPoints, prevBonusPoints);
                    totalPoints = roundService.JoinTotalResults(prevTotalPoints, totalPoints);
                }
            }

            var groupPoints = groupService.GetRoundResults();
            var groupBonusPoints = roundService.GetBonusResults(groupPoints);
            totalPoints = roundService.JoinTotalResults(roundService.GetTotalResults(groupPoints, groupBonusPoints), totalPoints);

            roundService.AddCurrentTimeDelimiter(ref matches);

            var model = new RoundViewModel()
            {
                Title = roundService.GetRoundTitle(id),
                Users = mappedUsers,
                Matches = matches,
                RoundPoints = roundPoints,
                BonusPoints = bonusPoints,
                TotalPoints = totalPoints
            };

            return View(model);
        }

    }
}