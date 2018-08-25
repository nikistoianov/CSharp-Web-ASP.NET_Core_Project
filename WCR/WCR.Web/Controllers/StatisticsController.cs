namespace WCR.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using WCR.Services.Statistics.Interfaces;

    public class StatisticsController : Controller
    {
        private readonly IStatisticsService statisticsService;
        public StatisticsController(IStatisticsService statisticsService)
        {
            this.statisticsService = statisticsService;
        }

        public IActionResult Index()
        {
            var model = statisticsService.GetAllUsersStatistics();
            return View(model);
        }

        public IActionResult User(string id)
        {
            var model = statisticsService.GetUserStatistics(id);
            return View(model);
        }
    }
}