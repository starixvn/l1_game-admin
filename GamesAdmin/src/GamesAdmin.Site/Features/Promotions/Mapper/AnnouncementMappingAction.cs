using System;
using AutoMapper;
using GamesAdmin.Core.Models;
using GamesAdmin.Site._Shared.Configurations;
using GamesAdmin.Site.Features.Promotions.ViewModels;

namespace GamesAdmin.Site.Features.Promotions.Mapper
{
    public class AnnouncementMappingAction : IMappingAction<Notification, AnnouncementViewModel>
    {
        private readonly IAppSettings appSettings;

        public AnnouncementMappingAction(IAppSettings appSettings)
        {
            this.appSettings = appSettings;
        }

        public void Process(Notification source, AnnouncementViewModel destination, ResolutionContext context)
        {
            destination.StartTime = TimeZoneInfo.ConvertTimeFromUtc(source.StartTime ?? DateTime.Now, appSettings.DefaultTimeZone);
            destination.EndTime = TimeZoneInfo.ConvertTimeFromUtc(source.EndTime ?? DateTime.Now, appSettings.DefaultTimeZone);
        }
    }
}
