namespace WCR.Tests.Mocks
{
    using Microsoft.EntityFrameworkCore;
    using System;
    using WCR.Data;

    public static class MockDbContext
    {
        public static  WCRDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<WCRDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            var dbContext = new WCRDbContext(options);
            return dbContext;
        }
    }
}
