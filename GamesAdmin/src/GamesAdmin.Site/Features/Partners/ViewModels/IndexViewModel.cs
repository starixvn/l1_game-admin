using System;
using System.Collections.Generic;
using System.Linq;
using GamesAdmin.Core.Models.Partners;
using GamesAdmin.Core.Models.Um;

namespace GamesAdmin.Site.Features.Partners.ViewModels
{
    public class IndexViewModel
    {
        public IList<Partner> Partners { get; set; }

        public IList<UmInfo> UmInfos { get; set; }

        public string SCUmInfo
        {
            get
            {
                var scUmInfo = UmInfos.FirstOrDefault(um => !um.IsPartnerUM);

                if (scUmInfo != null)
                {
                    return buildUmInfo(scUmInfo.Start, scUmInfo.End);
                }

                return null;
            }
        }

        public string GetPartnerUmInfo(string partnerId)
        {
            var partnerUMInfo = UmInfos.FirstOrDefault(um => um.IsPartnerUM && um.PartnerId == partnerId);

            if (partnerUMInfo != null)
            {
                return buildUmInfo(partnerUMInfo.Start, partnerUMInfo.End);
            }

            return null;
        }

        public bool EditPermission { get; set; }

        private string buildUmInfo(DateTimeOffset start, DateTimeOffset end) 
            => $"Start Time (GMT+7): {start.ToOffset(new TimeSpan(7, 0, 0))} \nEnd Time (GMT+7): {end.ToOffset(new TimeSpan(7, 0, 0))}";
    }
}