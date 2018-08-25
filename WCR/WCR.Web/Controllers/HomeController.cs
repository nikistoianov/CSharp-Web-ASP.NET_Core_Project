namespace WCR.Web.Controllers
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using AutoMapper;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using WCR.Common.Home.ViewModels;
    using WCR.Models;
    using WCR.Web.Models;

    public class HomeController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly IMapper mapper;

        public HomeController(UserManager<User> userManager, IMapper mapper)
        {
            this.userManager = userManager;
            this.mapper = mapper;
        }

        public IActionResult Index()
        {
            var users = userManager.Users
                .OrderBy(x => x.ShortName)
                .ToList();

            var mappedUsers = mapper.Map<List<UserHomeViewModel>>(users);
            return View(mappedUsers);
        }

        public IActionResult Rules()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
