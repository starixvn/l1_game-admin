using AutoMapper;
using GamesAdmin.Site.Features.ApiLogs.Models;
using GamesAdmin.Site.Features.ApiLogs.Requests;

namespace GamesAdmin.Site.Features.ApiLogs.Mappers
{
    public class ApiLogMapper : Profile
    {
        public ApiLogMapper()
        {
            CreateMap<QueryApiLogRequest, QueryLogModel>()
                .ConstructUsing(req => new QueryLogModel(
                    req.LogType,
                    req.PartnerId,
                    req.FromDate,
                    req.ToDate,
                    req.ObCustId,
                    req.GameRoundId,
                    req.GameTypeId,
                    req.Username,
                    req.SiteId));
        }
    }
}