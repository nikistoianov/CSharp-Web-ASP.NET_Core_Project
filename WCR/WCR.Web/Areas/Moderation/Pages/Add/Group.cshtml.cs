namespace WCR.Web.Areas.Moderation.Pages.Add
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using WCR.Common.Constants;
    using WCR.Common.Moderation.BindingModels;
    using WCR.Services.Moderation.Interfaces;

    [Authorize(Roles = Constants.ROLE_ADMIN + ", " + Constants.ROLE_MOD)]
    public class GroupModel : PageModel
    {
        private readonly IModerationService moderationService;

        public GroupModel(IModerationService moderationService)
        {
            this.Input = new GroupCreationBindingModel();
            this.moderationService = moderationService;
        }

        [BindProperty]
        public GroupCreationBindingModel Input { get; set; }

        public void OnGet()
        {
            this.Input.Date = DateTime.Now;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            if (!this.ModelState.IsValid)
            {
                return Page();
            }

            var result = await this.moderationService.CreateGroup(Input);
            if (result == null)
            {
                return Redirect(returnUrl ?? "/");
            }
            ModelState.AddModelError(string.Empty, result);
            return Page();
        }
    }
}