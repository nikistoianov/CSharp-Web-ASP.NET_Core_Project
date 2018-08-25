namespace WCR.Web.Areas.Administration.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using WCR.Common.Constants;
    using WCR.Models;
    using WCR.Web.Areas.Administration.Models.ViewModels;

    public class UsersController : AdminController
    {
        private readonly UserManager<User> userManager;
        public UsersController(UserManager<User> userManager)
        {
            this.userManager = userManager;
        }

        public ActionResult Index()
        {
            var model = new AdminUsersViewModel();
            var users = userManager.Users
                .OrderBy(x => x.ShortName)
                .ToList();

            foreach (var user in users)
            {
                var roles = userManager.GetRolesAsync(user);
                roles.Wait();

                var viewUser = UserListViewModel.FromUser(user);
                viewUser.Roles = roles.Result.ToArray();
                model.Users.Add(viewUser);
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UserToModerator(string id, string returnUrl = "/")
        {
            var user = await this.userManager.FindByIdAsync(id);

            var userRoles = userManager.GetRolesAsync(user);
            userRoles.Wait();

            if (!userRoles.Result.Contains(Constants.ROLE_MOD))
            {
                var newUserRole = userManager.AddToRoleAsync(user, Constants.ROLE_MOD);
                newUserRole.Wait();
            }

            return this.Redirect(returnUrl);
        }

        [HttpPost]
        public async Task<IActionResult> ModeratorToUser(string id, string returnUrl = "/")
        {
            var user = await this.userManager.FindByIdAsync(id);

            var userRoles = userManager.GetRolesAsync(user);
            userRoles.Wait();

            if (userRoles.Result.Contains(Constants.ROLE_MOD))
            {
                var newUserRole = userManager.RemoveFromRoleAsync(user, Constants.ROLE_MOD);
                newUserRole.Wait();
            }

            return this.Redirect(returnUrl);
        }

    }
}