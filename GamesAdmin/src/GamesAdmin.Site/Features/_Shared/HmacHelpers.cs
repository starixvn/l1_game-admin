using System;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace GamesAdmin.Site.Features._Shared
{
    public static class HmacHelpers
    {
        public static string CreateAccessKey(string secretKey, object request)
        {
            var data = JsonConvert.SerializeObject(request, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                StringEscapeHandling = StringEscapeHandling.EscapeHtml
            });
            Sentry.SentrySdk.CaptureMessage($"{data}");
            var hmac = new HMACSHA256(Encoding.ASCII.GetBytes(secretKey));
            var hashResult = hmac.ComputeHash(Encoding.ASCII.GetBytes(data));

            var result = Convert.ToBase64String(hashResult);
            return result;
        }
    }
}