namespace WCR.Web.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using WCR.Common.Competition.BindingModels;
    using WCR.Common.Constants;
    using WCR.Models;
    using WCR.Services.Competition.Interfaces;

    public class BetsMatchController : Controller
    {
        private readonly IBetService betService;
        private readonly UserManager<User> userManager;

        public BetsMatchController(IBetService betService, UserManager<User> userManager)
        {
            this.betService = betService;
            this.userManager = userManager;
        }

        [HttpGet]
        public IActionResult Create(int id)
        {
            var model = betService.PrepareBetMatch(id);
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BetMatchBindingModel model, int id, string returnUrl = "/")
        {
            if (!ModelState.IsValid)
            {
                this.ModelState.AddModelError(string.Empty, "Validation error.");
                return View(betService.PrepareBetMatch(id));
            }

            if (await betService.IsBeggined(false, id))
            {
                this.ModelState.AddModelError(string.Empty, "Time is out for prognosis.");
                return View(betService.PrepareBetMatch(id));
            }

            try
            {
                var currentUserId = userManager.GetUserId(this.User);
                var result = await betService.AddBetMatchAsync(id, currentUserId, model.HomeTeamGoals, model.GuestTeamGoals);

                return Redirect(returnUrl);
            }
            catch
            {
                return Redirect("/");
            }
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var model = betService.GetBetMatch(id);
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(BetMatchBindingModel model, int id, string returnUrl = "/")
        {
            if (!ModelState.IsValid)
            {
                this.ModelState.AddModelError(string.Empty, "Validation error.");
                return View(betService.GetBetMatch(id));
            }

            if (!this.User.IsInRole(Constants.ROLE_ADMIN) && await betService.IsBeggined(false, id))
            {
                this.ModelState.AddModelError(string.Empty, "Time is out for prognosis.");
                return View(betService.PrepareBetMatch(id));
            }

            try
            {
                var bet = betService.GetDbBetMatch(id);
                var currentUserId = userManager.GetUserId(this.User);
                if (currentUserId != bet.UserId && !this.User.IsInRole(Constants.ROLE_ADMIN))
                {
                    this.ModelState.AddModelError(string.Empty, "You are not allowed to edit this bet.");
                    return View(betService.GetBetMatch(id));
                }

                var result = await betService.EditBetMatchAsync(id, model.HomeTeamGoals, model.GuestTeamGoals);
                if (result != null)
                {
                    this.ModelState.AddModelError(string.Empty, result);
                    return View(betService.GetBetMatch(id));
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