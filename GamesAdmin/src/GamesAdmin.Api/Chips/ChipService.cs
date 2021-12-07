using System.Collections.Generic;
using System.Threading.Tasks;
using GamesAdmin.Api.Chips.Requests;
using GamesAdmin.Core.Models.Chips;
using MediatR;

namespace GamesAdmin.Api.Chips
{
    public interface IChipService
    {
        Task<IEnumerable<ChipModel>> GetAll();
    }

    public class ChipService : IChipService
    {
        private readonly IMediator mediator;

        public ChipService(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public Task<IEnumerable<ChipModel>> GetAll()
            => mediator.Send(new GetAllChipsRequest());
    }
}