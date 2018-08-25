namespace WCR.Tests.Mocks
{
    using AutoMapper;
    using WCR.Web.Mapping;

    public static class MockAutoMapper
    {
        static MockAutoMapper()
        {
            Mapper.Initialize(conf => conf.AddProfile<AutoMapperProfile>());
        }

        public static IMapper GetAutoMapper() => Mapper.Instance;
    }
}
