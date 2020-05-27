using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GamesAdmin.Core.Models;
using GamesAdmin.Site.Features._Shared;
using Refit;

namespace GamesAdmin.Site.Features.VietlottKeno
{
    public interface IVietlottKenoApi : IBaseAuthorizationApi
    {
        [Get("/vietlott_keno/rounds/{date}")]
        Task<IEnumerable<VietlottKenoRound>> GetRounds([Query(Format = "yyyy-MM-ddTHH:mm:ssZ")]DateTime date);

        [Post("/vietlott_keno/rounds/create")]
        Task Create([Body] CreateVietlottKenoRound kenoRound);
    }
}