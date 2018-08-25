namespace WCR.Web.Common
{
    using Microsoft.AspNetCore.Identity;
    using System.Linq;
    using System.Threading.Tasks;
    using WCR.Common.Constants;
    using WCR.Models;

    public static class UserManagerExtensions
    {
        public static bool IsFirstUser(this UserManager<User> userManager)
        {
            return !userManager.Users.Any();
        }

        public static async Task<string> MakeAdminIfFirst(this UserManager<User> userManager, User user)
        {
            if (userManager.Users.Count() == 1)
            {
                var result = await userManager.AddToRoleAsync(user, Constants.ROLE_ADMIN);
            }

            return "";
        }
    }
}
