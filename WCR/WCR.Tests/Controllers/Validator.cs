namespace WCR.Tests.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System.Linq;

    public static class Validator
    {

        public static bool ModelStateHasErrors(ViewResult viewResult)
        {
            var vals = viewResult.ViewData.ModelState.Values;
            foreach (var val in vals)
            {
                if (val.Errors.Count > 0)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool ModelStateHasError(ViewResult viewResult, string errorMessage)
        {
            var vals = viewResult.ViewData.ModelState.Values;
            foreach (var val in vals)
            {
                if (val.Errors.Count > 0 && val.Errors.Any(x => x.ErrorMessage == errorMessage))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
