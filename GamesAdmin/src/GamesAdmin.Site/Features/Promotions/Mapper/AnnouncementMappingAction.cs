using System;
using AutoMapper;
using GamesAdmin.Core.Models;
using GamesAdmin.Site._Shared.Configurations;
using GamesAdmin.Site.Features.Promotions.ViewModels;
using GamesAdmin.Core.Enumeration;
using System.Collections.Generic;

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
            
            // Ensure all language keys are present in the dictionaries
            EnsureAllLanguageKeys(destination.Title);
            EnsureAllLanguageKeys(destination.Messages);
        }

        private void EnsureAllLanguageKeys(Dictionary<string, string> dictionary)
        {
            if (dictionary == null)
            {
                dictionary = new Dictionary<string, string>();
            }

            foreach (var languageKey in Language.EmptyLanguages.Keys)
            {
                if (!dictionary.ContainsKey(languageKey))
                {
                    dictionary[languageKey] = string.Empty;
                }
            }
        }
    }
}
