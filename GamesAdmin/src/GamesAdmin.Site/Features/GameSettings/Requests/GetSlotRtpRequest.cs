using System.Collections.Generic;
using GamesAdmin.Core.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GamesAdmin.Site.Features.GameSettings.Requests
{
    public class GetSlotRtpRequest : IRequest<RtpResult>
    {
        public GetSlotRtpRequest(Dictionary<string, Dictionary<string, double>> odds, int[][] weight, int numOfRounds)
        {
            Odds = odds;
            Weight = weight;
            NumOfRounds = numOfRounds;
        }

        public Dictionary<string, Dictionary<string, double>> Odds { get; }
        public int[][] Weight { get; }
        public int NumOfRounds { get; }
    }
}