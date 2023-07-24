using System.Collections.Generic;
using MediatR;
using Newtonsoft.Json;

namespace GamesAdmin.Api.RTPVerify.Requests
{
    public class RTPVerifyRequest : IRequest<string>
    {
        [JsonConstructor]
        public RTPVerifyRequest(Dictionary<string, Dictionary<string, double>> odds, int[][] weight, int numOfRounds)
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