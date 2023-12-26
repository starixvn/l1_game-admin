using System;
using System.Security.Cryptography;
using System.Text;
using GamesAdmin.Core.Models.ApiLogs;
using GamesAdmin.Site.Features.BetDetails.ViewModels;
using Newtonsoft.Json;

namespace GamesAdmin.Site.Features._Shared
{
    public class BetHistoryApiRequest
    {
        public BetHistoryApiRequest(BetDetailsViewModel model)
        {
            Auth = new Auth
            {
                OperatorId = model.OperatorId,
                OperatorSecret = model.OperatorSecret
            };
            GameRoundId = model.RoundId;
            GameTypeId = model.GameType;
            PlayerId = model.PlayerId;
            FreeBet = model.FreeBet;
        }
        public int GameTypeId { get; set; }

        public Auth Auth { get; set; }

        public long GameRoundId { get; set; }

        public int PlayerId { get; set; }


        public bool FreeBet { get; set; }
    }

    public interface IHmacHelpers
    {
        string CreateAccessKey(string secretKey, BetHistoryApiRequest request);
    }

    public class HmacHelpers : IHmacHelpers
    {
        public string CreateAccessKey(string secretKey, BetHistoryApiRequest request)
        {
            var data = JsonConvert.SerializeObject(request);
            var hmac = new HMACSHA256(Encoding.ASCII.GetBytes(secretKey));
            var hashResult = hmac.ComputeHash(Encoding.ASCII.GetBytes(data));

            var result = Convert.ToBase64String(hashResult);
            return result;
        }
    }
}