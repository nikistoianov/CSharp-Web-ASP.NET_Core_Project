namespace WCR.Web.Areas.Moderation.Pages.Add
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using WCR.Common.Constants;
    using WCR.Common.Moderation.BindingModels;
    using WCR.Services.Moderation.Interfaces;

    [Authorize(Roles = Constants.ROLE_ADMIN + ", " + Constants.ROLE_MOD)]
    public class TeamModel : PageModel
    {
        private readonly IModerationService moderationService;

        public TeamModel(IModerationService moderationService)
        {
            this.Input = new TeamCreationBindingModel();
            this.moderationService = moderationService;
        }

        [BindProperty]
        public TeamCreationBindingModel Input { get; set; }

        public async void OnGet()
        {
            var model = await this.moderationService.PrepareTeamCreation();
            this.Input = model;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            if (!this.ModelState.IsValid)
            {
                return Page();
            }

            var result = await this.moderationService.CreateTeam(Input);
            if (result == null)
            {
                return Redirect(returnUrl ?? "/");
            }
            ModelState.AddModelError(string.Empty, result);
            return Page();
        }
    }
}