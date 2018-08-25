namespace WCR.Web.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using WCR.Common.Competition.BindingModels;
    using WCR.Common.Constants;
    using WCR.Services.Moderation.Interfaces;

    [Authorize(Roles = Constants.ROLE_ADMIN + ", " + Constants.ROLE_MOD)]
    public class GroupsController : Controller
    {
        private readonly IModerationService moderationService;

        public GroupsController(IModerationService moderationService)
        {
            this.moderationService = moderationService;
        }

        [HttpGet]
        public IActionResult EditPosition(int id)
        {
            var model = moderationService.PrepareGroupScore(id);
            if (model == null)
            {
                this.ModelState.AddModelError(string.Empty, "Group not found.");
                return View(new BetGroupBindingModel());
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPosition(BetGroupBindingModel model, int id, string returnUrl = "/")
        {
            if (!ModelState.IsValid)
            {
                this.ModelState.AddModelError(string.Empty, "Validation error.");
                return View(moderationService.PrepareGroupScore(id));
            }

            var distTeams = model.Teams.Select(x => x.Position).Distinct().ToArray();
            if (distTeams.Length != model.Teams.Count)
            {
                this.ModelState.AddModelError(string.Empty, "No dublicate positions are allowed.");
                return View(moderationService.PrepareGroupScore(id));
            }

            try
            {
                var result = await moderationService.EditGroupScoreAsync(id, model);
                if (result != null)
                {
                    this.ModelState.AddModelError(string.Empty, result);
                    return View(new BetGroupBindingModel());
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