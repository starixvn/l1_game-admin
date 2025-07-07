using System.Collections.Generic;
using GamesAdmin.Core.Models;
using MediatR;

namespace GamesAdmin.Api.CurrencyGroupGames.Requests
{
    public class SuggestNewRangeRequest : IRequest<IEnumerable<CurrencyGroupGame>>
    {
        public SuggestNewRangeRequest(CurrencyGroupProposal proposal)
        {
            Proposal = proposal;
        }

        public CurrencyGroupProposal Proposal { get; }
    }
} 