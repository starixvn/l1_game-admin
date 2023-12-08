using System.Threading.Tasks;
using GamesAdmin.Core.Models.Partners;
using Refit;

namespace GamesAdmin.Api.PartnerRemote
{
    [Headers("Authorization: Bearer")]
    public interface IPartnerRemoteApi
    {
        [Post("/api/partner/add")]
        Task<bool> Create(Partner partner);

        [Post("/api/partner/update")]
        Task<bool> Update(Partner partner);
    }
}
