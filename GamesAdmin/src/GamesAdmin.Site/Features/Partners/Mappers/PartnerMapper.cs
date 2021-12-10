using AutoMapper;
using GamesAdmin.Core.Models.Partners;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GamesAdmin.Site.Features.Partners.Mappers
{
    public class PartnerMapper : Profile
    {
        public PartnerMapper()
        {
            CreateMap<Partner, SelectListItem>()
                .ConstructUsing(partner => new SelectListItem(partner.Name, partner.PartnerId));
        }
    }
}