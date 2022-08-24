using AutoMapper;
using GamesAdmin.Core.Models;
using GamesAdmin.Site.Features.Promotions.ViewModels;
using GamesAdmin.Site.Extensions;
using System.Linq;

namespace GamesAdmin.Site.Features.Promotions.Mapper
{
    public class PromotionMapper : Profile
    {
        public PromotionMapper()
        {
            CreateMap<PromotionViewModel, PromotionModel>()
                .ForMember(dest => dest.PromotionId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Odds, opt => opt.MapFrom(src => src.ListOdds.ToDictionary(odds => odds.Name, odds => odds.Odds)))
                .ForMember(dest => dest.StartDateTime,
                    opt => opt.MapFrom(src => DateTimeExtensions.ParseDateTime(src.StartDate, src.StartTime)))
                .ForMember(dest => dest.EndDateTime,
                    opt => opt.MapFrom(src => DateTimeExtensions.ParseDateTime(src.EndDate, src.EndTime)));

            CreateMap<PromotionModel, PromotionViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.PromotionId))
                .ForMember(dest => dest.Enabled, opt => opt.MapFrom(src => src.Status.Equals("enabled")))
                .AfterMap<PromotionMappingAction>();

            CreateMap<PromotionModel, UpdatePromotionViewModel>()
                .IncludeBase<PromotionModel, PromotionViewModel>();

            CreateMap<UpdatePromotionViewModel, PromotionModel>()
               .IncludeBase<PromotionViewModel, PromotionModel>();

            CreateMap<CreateAnnouncementItemViewModel, Notification>();

            CreateMap<Notification, CreateAnnouncementItemViewModel>();

            CreateMap<Notification, AnnouncementViewModel>()
                .AfterMap<AnnouncementMappingAction>();
        }
    }
}
