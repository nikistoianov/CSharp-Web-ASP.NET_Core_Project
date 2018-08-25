namespace WCR.Web.Areas.Administration.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using WCR.Common.Constants;

    [Area("Administration")]
    [Authorize(Roles = Constants.ROLE_ADMIN)]
    public abstract class AdminController : Controller
    {
    }
}
