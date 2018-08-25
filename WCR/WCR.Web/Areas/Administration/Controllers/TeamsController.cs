namespace WCR.Web.Areas.Administration.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using WCR.Services.Administration.Interfaces;

    public class TeamsController : AdminController
    {
        private readonly IAdminService adminService;
        public TeamsController(IAdminService adminService)
        {
            this.adminService = adminService;
        }

        public IActionResult Index()
        {
            var model = adminService.GetTeams();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, string returnUrl)
        {
            try
            {
                var result = await this.adminService.DeleteTeamAsync(id);
                if (result != null)
                {
                    this.ModelState.AddModelError(string.Empty, result);
                    return View(nameof(Index));
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                this.ModelState.AddModelError(string.Empty, ex.Message);
                return View(nameof(Index));
            }
        }
    }
}