namespace GamesAdmin.Site.Features.BetDetails
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using GamesAdmin.Site.Features.BetDetails.Requests;
    using GamesAdmin.Site.Features.BetDetails.ViewModels;
    using GamesAdmin.Site.Features.Partners;
    using MediatR;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class BetDetailsHandler : IRequestHandler<BetDetailsPageRequest, BetDetailsViewModel>, IRequestHandler<GetUrlAccessBetDetailsRequest, string>
    {
        private readonly IPartnerService operatorService;
        private readonly IBetDetailsService betDetailsService;

        public BetDetailsHandler(IPartnerService operatorService, IBetDetailsService betDetailsService)
        {
            this.operatorService = operatorService;
            this.betDetailsService = betDetailsService;
        }

        public async Task<BetDetailsViewModel> Handle(BetDetailsPageRequest request, CancellationToken cancellationToken)
        {
            var operators = await operatorService.GetAll();
            var languageItems = new List<SelectListItem> {
                    new SelectListItem
                    {
                        Text = "English",
                        Value = "en-US"
                    },
                    new SelectListItem
                    {
                        Text = "Vietnamese",
                        Value = "vi-VN"
                    },
                    new SelectListItem
                    {
                        Text = "Indonesia",
                        Value = "id-ID"
                    },
                    new SelectListItem
                    {
                        Text = "Thailand",
                        Value = "th-TH"
                    },
                    new SelectListItem
                    {
                        Text = "Chinese Simplified",
                        Value = "zh-CN"
                    },
                    new SelectListItem
                    {
                        Text = "Chinese Traditional",
                        Value = "zh-TW"
                    },
                    new SelectListItem
                    {
                        Text = "Japanese",
                        Value = "ja-JP"
                    },
                    new SelectListItem
                    {
                        Text = "Spanish",
                        Value = "es-ES"
                    },
                    new SelectListItem
                    {
                        Text = "Turkish",
                        Value = "tr-TR"
                    },
                    new SelectListItem
                    {
                        Text = "Philippines",
                        Value = "fil-PH"
                    },
                    new SelectListItem
                    {
                        Text = "Portuguese",
                        Value = "pt-PT"
                    },
                    new SelectListItem
                    {
                        Text = "Korean",
                        Value = "ko-KR"
                    }
                };

            return new BetDetailsViewModel
            {
                LanguageItem = languageItems,
                Operators = operators,
            };
        }

        public async Task<string> Handle(GetUrlAccessBetDetailsRequest request, CancellationToken cancellationToken)
        {
            var Operator = await this.operatorService.GetById(request.Model.OperatorId);
            if (Operator != null)
            {
                request.Model.OperatorSecret = Operator.SecretKey;
            }
            return await betDetailsService.GetAccessUrlAsync(request.Model);
        }
    }
}
