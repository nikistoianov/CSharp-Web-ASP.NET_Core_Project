namespace WCR.Tests.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System.Threading.Tasks;
    using WCR.Common.Competition.BindingModels;
    using WCR.Services.Moderation.Interfaces;
    using WCR.Web.Controllers;

    [TestClass]
    public class MatchesTests
    {
        [TestMethod]
        public void GetEditScore_WithMissingId_ReturnsError()
        {
            var mockService = new Mock<IModerationService>();

            var controller = new MatchesController(mockService.Object);

            var result = controller.EditScore(1);
            var viewResult = result as ViewResult;

            Assert.IsTrue(Validator.ModelStateHasError(viewResult, "Match not found."));
        }

        [TestMethod]
        public void GetEditScore_WithCorrectId_ReturnsModel()
        {
            var mockService = new Mock<IModerationService>();
            mockService.Setup(x => x.PrepareMatchScore(1))
                .Returns(new BetMatchBindingModel());

            var controller = new MatchesController(mockService.Object);

            var result = controller.EditScore(1);
            var viewResult = result as ViewResult;

            Assert.IsNotNull(viewResult.Model);
        }

        [TestMethod]
        public async Task PostEditScore_WithInvalidStateModel_ReturnsError()
        {
            var mockService = new Mock<IModerationService>();

            var controller = new MatchesController(mockService.Object);
            controller.ModelState.AddModelError(string.Empty, "Some Error");

            var result = controller.EditScore(null, 1);
            var viewResult = await result as ViewResult;

            Assert.IsTrue(Validator.ModelStateHasErrors(viewResult));
        }
    }
}
