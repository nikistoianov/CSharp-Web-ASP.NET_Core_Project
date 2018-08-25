namespace WCR.Web.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using WCR.Common.Competition.BindingModels;
    using WCR.Common.Constants;
    using WCR.Models;
    using WCR.Services.Competition.Interfaces;

    public class BetsGroupController : Controller
    {
        private readonly IBetService betService;
        private readonly UserManager<User> userManager;

        public BetsGroupController(IBetService betService, UserManager<User> userManager)
        {
            this.betService = betService;
            this.userManager = userManager;
        }

        [HttpGet]
        public IActionResult Create(int id)
        {
            var model = betService.PrepareBetGroup(id);
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BetGroupBindingModel model, int id, string returnUrl = "/")
        {
            if (!ModelState.IsValid)
            {
                this.ModelState.AddModelError(string.Empty, "Validation error.");
                return View(betService.PrepareBetGroup(id));
            }

            if (await betService.IsBeggined(true, id))
            {
                this.ModelState.AddModelError(string.Empty, "Time is out for prognosis.");
                return View(betService.PrepareBetGroup(id));
            }

            var distTeams = model.Teams.Select(x => x.Position).Distinct().ToArray();
            if (distTeams.Length != model.Teams.Count)
            {
                this.ModelState.AddModelError(string.Empty, "No dublicate positions are allowed.");
                return View(betService.PrepareBetGroup(id));
            }

            try
            {
                var currentUserId = userManager.GetUserId(this.User);
                var result = await betService.AddBetGroupAsync(currentUserId, model);
                if (result != null)
                {
                    this.ModelState.AddModelError(string.Empty, result);
                    return View(betService.PrepareBetGroup(id));
                }

                return Redirect(returnUrl);
            }
            catch
            {
                this.ModelState.AddModelError(string.Empty, "Error creating prognosis.");
                return View(betService.PrepareBetGroup(id));
            }
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var currentUserId = userManager.GetUserId(this.User);
            var model = betService.GetBetGroup(id, currentUserId);
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(BetGroupBindingModel model, int id, string returnUrl = "/")
        {
            if (!ModelState.IsValid)
            {
                this.ModelState.AddModelError(string.Empty, "Validation error.");
                return View(betService.GetBetMatch(id));
            }
            
            if (!this.User.IsInRole(Constants.ROLE_ADMIN) && await betService.IsBeggined(true, id))
            {
                this.ModelState.AddModelError(string.Empty, "Time is out for prognosis.");
                return View(betService.PrepareBetGroup(id));
            }

            var distTeams = model.Teams.Select(x => x.Position).Distinct().ToArray();
            if (distTeams.Length != model.Teams.Count)
            {
                this.ModelState.AddModelError(string.Empty, "No dublicate positions are allowed.");
                return View(betService.PrepareBetGroup(id));
            }

            try
            {
                var currentUserId = userManager.GetUserId(this.User);
                var result = await betService.EditBetGroupAsync(currentUserId, model);
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