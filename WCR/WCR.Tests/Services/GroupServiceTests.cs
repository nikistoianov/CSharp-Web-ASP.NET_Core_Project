namespace WCR.Tests.Services
{
    using AutoMapper;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Linq;
    using WCR.Data;
    using WCR.Models;
    using WCR.Services.Competition;
    using WCR.Tests.Mocks;
    using WCR.Common.Constants;
    using System.Reflection;
    using WCR.Common.Competition.ViewModels;
    using System.Collections.Generic;
    using WCR.Common.Administration.ViewModels;

    [TestClass]
    public class GroupServiceTests
    {
        private WCRDbContext dbContext;
        private IMapper mapper;

        [TestMethod]
        public void GetGroups_WithFewGrous_ShoudReturnAll()
        {
            this.dbContext.Groups.Add(new Group() { Date = DateTime.Now, Name = "A" });
            this.dbContext.Groups.Add(new Group() { Date = DateTime.Now.AddDays(-1), Name = "B" });
            this.dbContext.Groups.Add(new Group() { Date = DateTime.Now.AddDays(1), Name = "C" });
            this.dbContext.SaveChanges();

            var service = new GroupService(this.dbContext, this.mapper);

            var groups = service.GetGroups();

            Assert.IsNotNull(groups);
            Assert.AreEqual(3, groups.Count);
            CollectionAssert.AreEqual(new[] { "B", "A", "C"}, groups.Select(x => x.Name).ToArray());
        }

        [TestMethod]
        public void CalculatePoints()
        {
            var service = new GroupService(null, null);
            var type = typeof(GroupService);
            var method = type.GetMethod("CalculatePoints", BindingFlags.NonPublic | BindingFlags.Instance);

            Assert.AreEqual(Constants.POINTS_GROUP_1, method.Invoke(service, new object[] { 1, 1 }));
            Assert.AreEqual(Constants.POINTS_GROUP_2, method.Invoke(service, new object[] { 2, 2 }));
            Assert.AreEqual(Constants.POINTS_GROUP_3, method.Invoke(service, new object[] { 3, 3 }));
            Assert.AreEqual(Constants.POINTS_GROUP_4, method.Invoke(service, new object[] { 4, 4 }));
            Assert.AreEqual(0, method.Invoke(service, new object[] { null, 4 }));
            Assert.AreEqual(0, method.Invoke(service, new object[] { 1, 2 }));
        }

        [TestMethod]
        public void ArrangeTeamBets_WithMissing()
        {
            var users = new List<UserDetailsViewModel>();
            users.Add(new UserDetailsViewModel() { Id = "1" });
            users.Add(new UserDetailsViewModel() { Id = "2" });
            users.Add(new UserDetailsViewModel() { Id = "3" });

            var groups = new List<GroupViewModel>();
            groups.Add(new GroupViewModel()
            {
                Teams = new List<GroupTeamViewModel>()
                {
                    GetTeamFromBets(3, 2),
                }
            });

            var service = new GroupService(dbContext, null);
            service.ArrangeTeamBets(groups, users, "", false);

            CollectionAssert.AreEqual(new[] { Constants.NO_SCORE, " (2)", " (3)"}, groups[0].Teams.First().Bets.Select(x => x.Score).ToArray());
        }

        [TestMethod]
        public void ArrangeTeamBets_WithNoBets()
        {
            var users = new List<UserDetailsViewModel>();
            users.Add(new UserDetailsViewModel() { Id = "1" });
            users.Add(new UserDetailsViewModel() { Id = "2" });

            var groups = new List<GroupViewModel>();
            groups.Add(new GroupViewModel()
            {
                Teams = new List<GroupTeamViewModel>()
                {
                    new GroupTeamViewModel()
                }
            });

            var service = new GroupService(dbContext, null);
            service.ArrangeTeamBets(groups, users, "", false);

            CollectionAssert.AreEqual(new[] { Constants.NO_SCORE, Constants.NO_SCORE }, groups[0].Teams.First().Bets.Select(x => x.Score).ToArray());
        }

        private GroupTeamViewModel GetTeamFromBets(params int[] points)
        {
            var result = new GroupTeamViewModel();
            foreach (var point in points)
            {
                result.Bets.Add(new TeamBetViewModel() { UserId = point.ToString(), Points = point });
            }
            return result;
        }

        [TestMethod]
        public void GetRoundResults_WithEmptyBets_SHoudReturnZeroes()
        {
            this.dbContext.Users.Add(new User() { ShortName = "first"});
            this.dbContext.Users.Add(new User() { ShortName = "second"});
            this.dbContext.SaveChanges();

            var service = new GroupService(this.dbContext, this.mapper);
            var results = service.GetRoundResults();

            CollectionAssert.AreEqual(new int[] { 0, 0 }, results.Select(x => x.Points).ToArray());
        }

        [TestMethod]
        public void GetRoundResults_WithFewBets()
        {
            this.dbContext.Users.Add(new User() { ShortName = "one" });
            this.dbContext.Users.Add(new User() { ShortName = "two" });
            this.dbContext.SaveChanges();

            this.dbContext.Groups.Add(new Group()
            {
                Teams = new List<Team>()
                {
                    new Team(){Name = "1", GroupPosition = 1, BetsForPosition = new List<BetPosition> { new BetPosition() { Position = 1, UserId = this.dbContext.Users.First().Id} } },
                    new Team(){Name = "2", GroupPosition = 2, BetsForPosition = new List<BetPosition> { new BetPosition() { Position = 2, UserId = this.dbContext.Users.Last().Id} } }
                }
            });
            this.dbContext.SaveChanges();

            var service = new GroupService(this.dbContext, this.mapper);
            var results = service.GetRoundResults();

            CollectionAssert.AreEqual(new int[] { Constants.POINTS_GROUP_1, Constants.POINTS_GROUP_2 }, results.Select(x => x.Points).ToArray());
        }

        [TestMethod]
        public void ArrangeTeamBets_WithMixedBets()
        {
            var users = new List<UserDetailsViewModel>();
            users.Add(new UserDetailsViewModel() { Id = "1" });
            users.Add(new UserDetailsViewModel() { Id = "2" });
            users.Add(new UserDetailsViewModel() { Id = "3" });

            var groups = new List<GroupViewModel>();
            groups.Add(new GroupViewModel()
            {
                Teams = new List<GroupTeamViewModel>()
                {
                    GetTeamFromBets(1, 3, 2),
                }
            });

            var service = new GroupService(dbContext, null);
            service.ArrangeTeamBets(groups, users, "", false);

            CollectionAssert.AreEqual(new[] { " (1)", " (2)", " (3)" }, groups[0].Teams.First().Bets.Select(x => x.Score).ToArray());
        }

        [TestInitialize]
        public void InitializeTests()
        {
            this.dbContext = MockDbContext.GetDbContext();
            this.mapper = MockAutoMapper.GetAutoMapper();
        }
    }
}
