namespace WCR.Web.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using WCR.Common.Competition.BindingModels;
    using WCR.Common.Constants;
    using WCR.Services.Moderation.Interfaces;

    [Authorize(Roles = Constants.ROLE_ADMIN + ", " + Constants.ROLE_MOD)]
    public class MatchesController : Controller
    {
        private readonly IModerationService moderationService;

        public MatchesController(IModerationService moderationService)
        {
            this.moderationService = moderationService;
        }

        [HttpGet]
        public IActionResult EditScore(int id)
        {
            var model = moderationService.PrepareMatchScore(id);
            if (model == null)
            {
                this.ModelState.AddModelError(string.Empty, "Match not found.");
                return View(new BetMatchBindingModel());
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditScore(BetMatchBindingModel model, int id, string returnUrl = "/")
        {
            if (!ModelState.IsValid)
            {
                this.ModelState.AddModelError(string.Empty, "Validation error.");
                return View(moderationService.PrepareMatchScore(id));
            }

            try
            {
                var result = await moderationService.EditMatchScoreAsync(id, model.HomeTeamGoals, model.GuestTeamGoals);
                if (result != null)
                {
                    this.ModelState.AddModelError(string.Empty, result);
                    return View(new BetMatchBindingModel());
                }

                return Redirect(returnUrl);
            }
            catch
            {
                return Redirect("/");
            }
        }
    }
}