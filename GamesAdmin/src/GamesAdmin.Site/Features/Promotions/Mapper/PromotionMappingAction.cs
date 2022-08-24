using System;
using AutoMapper;
using GamesAdmin.Core.Models;
using GamesAdmin.Site._Shared.Configurations;
using GamesAdmin.Site.Features.Promotions.ViewModels;

namespace GamesAdmin.Site.Features.Promotions.Mapper
{
    public class PromotionMappingAction : IMappingAction<PromotionModel, PromotionViewModel>
    {
        private readonly IAppSettings appSettings;

        public PromotionMappingAction(IAppSettings appSettings)
        {
            this.appSettings = appSettings;
        }

        public void Process(PromotionModel source, PromotionViewModel destination, ResolutionContext context)
        {
            destination.StartDate = TimeZoneInfo.ConvertTimeFromUtc(source.StartDateTime, appSettings.DefaultTimeZone);
            destination.StartTime = TimeZoneInfo.ConvertTimeFromUtc(source.StartDateTime, appSettings.DefaultTimeZone);
            destination.EndDate = TimeZoneInfo.ConvertTimeFromUtc(source.EndDateTime, appSettings.DefaultTimeZone);
            destination.EndTime = TimeZoneInfo.ConvertTimeFromUtc(source.EndDateTime, appSettings.DefaultTimeZone);
        }
    }
}
