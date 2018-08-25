namespace WCR.Services
{
    using AutoMapper;
    using WCR.Data;

    public abstract class BaseEFService
    {
        protected BaseEFService(
            WCRDbContext dbContext,
            IMapper mapper)
        {
            this.DbContext = dbContext;
            this.Mapper = mapper;
        }

        protected WCRDbContext DbContext { get; private set; }

        protected IMapper Mapper { get; private set; }
    }
}
