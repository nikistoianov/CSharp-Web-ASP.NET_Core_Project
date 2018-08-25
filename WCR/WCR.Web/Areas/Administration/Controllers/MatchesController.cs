namespace WCR.Web.Areas.Administration.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using WCR.Services.Administration.Interfaces;

    public class MatchesController : AdminController
    {
        private readonly IAdminService adminService;
        public MatchesController(IAdminService adminService)
        {
            this.adminService = adminService;
        }

        public ActionResult Index()
        {
            var model = adminService.GetMatches();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await this.adminService.DeleteMatchAsync(id);
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