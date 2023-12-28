using System.Threading.Tasks;
using GamesAdmin.Core.Models.Partners;
using Refit;

namespace GamesAdmin.Api.PartnerRemote
{
    public interface IPartnerRemoteApi
    {
        [Post("/api/partner/add")]
        Task<bool> Create(Partner partner, [Header("Authorization")] string header = "");

        [Post("/api/partner/update")]
        Task<bool> Update(Partner partner, [Header("Authorization")] string header = "");
    }
}