using MediatR;

namespace GamesAdmin.Api.GameSettings.Requests
{
    public class UpdateSiteExclusionRequest : IRequest<bool>
    {
        public UpdateSiteExclusionRequest(string name, string[] exclusions) 
        {
            Name = name;
            Exclusions = exclusions;
        }

        public string Name { get; set; }

        public string[] Exclusions { get; set; }
    }
}