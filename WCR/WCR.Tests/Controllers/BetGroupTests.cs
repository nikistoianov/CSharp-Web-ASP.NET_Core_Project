namespace WCR.Tests.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using WCR.Common.Competition.BindingModels;
    using WCR.Services.Competition.Interfaces;
    using WCR.Web.Controllers;

    [TestClass]
    public class BetGroupTests
    {
        [TestMethod]
        public void GetCreate_WithNothing_ShoudReturnNotFound()
        {
            var mockService = new Mock<IBetService>();

            var controller = new BetsGroupController(mockService.Object, null);

            var result = controller.Create(1);

            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void GetCreate_WithValidGroup_ShoudReturnModel()
        {
            var mockService = new Mock<IBetService>();
            mockService.Setup(x => x.PrepareBetGroup(1))
                .Returns(new BetGroupBindingModel());

            var controller = new BetsGroupController(mockService.Object, null);

            var result = controller.Create(1);

            Assert.IsNotNull((result as ViewResult).Model);
        }

        [TestMethod]
        public async Task PostCreate_ForLateBet_ReturnsError()
        {
            var mockService = new Mock<IBetService>();
            mockService.Setup(x => x.IsBeggined(true, 1))
                .ReturnsAsync(true);

            var controller = new BetsGroupController(mockService.Object, null);

            var result = await controller.Create(null, 1);
            var viewResult = result as ViewResult;

            Assert.IsTrue(Validator.ModelStateHasErrors(viewResult));
            Assert.IsTrue(Validator.ModelStateHasError(viewResult, "Time is out for prognosis."));
        }

        [TestMethod]
        public async Task PostCreate_ForDublicatedPositions_ReturnsError()
        {
            var mockService = new Mock<IBetService>();

            var controller = new BetsGroupController(mockService.Object, null);

            var model = new BetGroupBindingModel()
            {
                Teams = new List<BetTeamBindingModel>()
                {
                    new BetTeamBindingModel() { Position = 1},
                    new BetTeamBindingModel() { Position = 1}
                }
            };

            var result = await controller.Create(model, 1);
            var viewResult = result as ViewResult;

            Assert.IsTrue(Validator.ModelStateHasError(viewResult, "No dublicate positions are allowed."));
        }

    }
}
