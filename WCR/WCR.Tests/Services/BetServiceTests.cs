namespace WCR.Tests.Services
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using WCR.Common.Competition.BindingModels;
    using WCR.Data;
    using WCR.Models;
    using WCR.Services.Competition;
    using WCR.Tests.Mocks;

    [TestClass]
    public class BetServiceTests
    {
        private WCRDbContext dbContext;

        [TestMethod]
        public async Task IsBeggined_ForStartedMatch_ShoudReturnTrue()
        {
            var bet = new BetMatch() { Match = new Match() { Date = DateTime.Now.AddDays(-1) } };
            this.dbContext.BetsForMatch.Add(bet);
            this.dbContext.SaveChanges();

            var service = new BetService(this.dbContext, null);

            var result = await service.IsBeggined(false, bet.Id);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task IsBeggined_ForTodayMatch_ShoudReturnTrue()
        {
            var bet = new BetMatch() { Match = new Match() { Date = DateTime.Now } };
            this.dbContext.BetsForMatch.Add(bet);
            this.dbContext.SaveChanges();

            var service = new BetService(this.dbContext, null);

            var result = await service.IsBeggined(false, bet.Id);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task IsBeggined_ForFutureMatch_ShoudReturnFalse()
        {
            var bet = new BetMatch() { Match = new Match() { Date = DateTime.Now.AddDays(1) } };
            this.dbContext.BetsForMatch.Add(bet);
            this.dbContext.SaveChanges();

            var service = new BetService(this.dbContext, null);

            var result = await service.IsBeggined(false, bet.Id);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task AddBetGroup_WithFewTeams_AddsCorrectNumber()
        {
            var model = new BetGroupBindingModel()
            {
                Teams = new List<BetTeamBindingModel>()
                {
                    new BetTeamBindingModel(){ Position = 1}
                }
            };

            var service = new BetService(this.dbContext, null);

            var result = await service.AddBetGroupAsync(null, model);
            var betsCount = this.dbContext.BetsForPosition.Count();

            Assert.AreEqual(model.Teams.Count, betsCount);
        }

        [TestMethod]
        public async Task AddBetGroup_WithNoTeams_AddsNothing()
        {
            var service = new BetService(this.dbContext, null);

            var result = await service.AddBetGroupAsync(null, new BetGroupBindingModel());
            var betsCount = this.dbContext.BetsForPosition.Count();

            Assert.AreEqual(0, betsCount);
        }

        [TestMethod]
        public void PrepareBetGroup_WithMissingGroupId_ShoudReturnNull()
        {
            var service = new BetService(this.dbContext, null);

            var result = service.PrepareBetGroup(1);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void PrepareBetGroup_WithExistingGroupId_ShoudReturnModel()
        {
            var group = new Group();
            this.dbContext.Groups.Add(group);
            this.dbContext.SaveChanges();

            var service = new BetService(this.dbContext, null);

            var result = service.PrepareBetGroup(group.Id);

            Assert.IsNotNull(result);
        }

        [TestInitialize]
        public void InitializeTests()
        {
            this.dbContext = MockDbContext.GetDbContext();
        }
    }
}
