namespace GamesAdmin.Site.Features.RetrieveEndRound
{
    using System.Threading.Tasks;
    using GamesAdmin.Site.Features.RetrieveEndRound.Model;
    using Refit;
    public interface IRetrieveEndRoundApi
    {
        [Post("/Member/RetrieveEndGameInfo")]
        Task<dynamic> RetrieveEndRound([Body(BodySerializationMethod.Serialized)] RetrieveEndRoundRequest request);
    }
}
