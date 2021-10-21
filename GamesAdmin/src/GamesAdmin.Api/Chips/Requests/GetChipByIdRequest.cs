using GamesAdmin.Core.Models.Chips;
using MediatR;

namespace GamesAdmin.Api.Chips.Requests
{
    public class GetChipByIdRequest : IRequest<ChipModel>
    {
        public string Id { get; }

        public GetChipByIdRequest(string id)
        {
            Id = id;
        } 
    }
}