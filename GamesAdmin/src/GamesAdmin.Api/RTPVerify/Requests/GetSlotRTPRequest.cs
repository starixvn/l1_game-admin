using System.Collections.Generic;
using GamesAdmin.Core.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GamesAdmin.Api.RTPVerify.Requests
{
    public class GetSlotRTPRequest : IRequest<RtpResult>
    {
        [JsonConstructor]
        public GetSlotRTPRequest(Dictionary<string, Dictionary<string, double>> odds, int[][] weight, int numOfRounds)
        {
            this.weight = weight;
            this.odds = odds;
            this.numOfRounds = numOfRounds;
        }

        public int[][] weight { get; set; }
        public Dictionary<string, Dictionary<string, double>> odds { get; set; }
        public int numOfRounds { get; set; }
    }
}