namespace WCR.Web.Common
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Configuration;
    using System.Linq;

    public static class ApplicationBuilderAuthExtensions
    {
        public static void SeedRoles(this IApplicationBuilder app, IConfiguration configuration,
            RoleManager<IdentityRole> roleManager)
        {
            var roles = configuration.GetSection("Roles").AsEnumerable().Select(x => x.Value).Skip(1).ToArray();
            CreateRoles(roleManager, roles);
        }


        private static void CreateRoles(RoleManager<IdentityRole> roleManager, string[] roles)
        {
            foreach (var role in roles)
            {
                CreateRole(roleManager, role);
            }
        }

        private static void CreateRole(RoleManager<IdentityRole> roleManager, string roleName)
        {
            var hasRole = roleManager.RoleExistsAsync(roleName);
            hasRole.Wait();

            if (!hasRole.Result)
            {
                var roleResult = roleManager.CreateAsync(new IdentityRole(roleName));
                roleResult.Wait();
            }
        }
    }
}
