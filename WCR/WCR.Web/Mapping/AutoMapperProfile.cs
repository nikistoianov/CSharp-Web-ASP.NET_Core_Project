namespace WCR.Web.Mapping
{
    using AutoMapper;
    using WCR.Common.Competition.ViewModels;
    using WCR.Common.Home.ViewModels;
    using WCR.Common.Moderation.BindingModels;
    using WCR.Models;

    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            this.CreateMap<User, UserDetailsViewModel>();
            this.CreateMap<User, UserHomeViewModel>();
            this.CreateMap<Group, GroupCreationBindingModel>().ReverseMap();
            this.CreateMap<Team, TeamCreationBindingModel>().ReverseMap();
            this.CreateMap<Match, MatchCreationBindingModel>().ReverseMap();
        }
    }
}
