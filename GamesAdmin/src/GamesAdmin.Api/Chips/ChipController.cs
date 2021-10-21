using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Api._Shared;
using GamesAdmin.Api.Chips.Requests;
using GamesAdmin.Core.Models.Chips;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GamesAdmin.Api.Chips
{
    [Route("/api/chip")]
    public class ChipController : BaseAuthorizeController
    {
        private readonly IMediator mediator;
        private readonly IMapper mapper;

        public ChipController(IMediator mediator, IMapper mapper)
        {
            this.mediator = mediator;
            this.mapper = mapper;
        }

        [HttpGet("all")]
        public Task<IEnumerable<ChipModel>> GetAll()
            => mediator.Send(new GetAllChipsRequest());

        [HttpGet("{id}")]
        public Task<ChipModel> GetById(string id)
            => mediator.Send(new GetChipByIdRequest(id));

        [HttpPost]
        public Task<bool> Create(CreateChipModel model)
            => mediator.Send(mapper.Map<CreateChipRequest>(model));

        [HttpPut]
        public Task<bool> Update(UpdateChipModel model)
            => mediator.Send(mapper.Map<UpdateChipRequest>(model));
    }
}