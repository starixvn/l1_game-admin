using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Api._Shared;
using GamesAdmin.Api.BuyFreeSpins.Requests;
using GamesAdmin.Api.BuyFreeSpins.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GamesAdmin.Api.BuyFreeSpins
{
    [Route("/api/BuyFreeSpin")]
    public class BuyFreeSpinController : BaseAuthorizeController
    {
        private readonly IMediator mediator;
        private readonly IMapper mapper;

        public BuyFreeSpinController(IMediator mediator, IMapper mapper)
        {
            this.mediator = mediator;
            this.mapper = mapper;
        }

        [HttpGet("all")]
        public Task<IEnumerable<BuyFreeSpinSettingViewModel>> GetAll()
        {
            var result = mediator.Send(new GetAllBuyFreeSpinsRequest());
            return result;
        }

        [HttpGet("{gameId}")]
        public Task<BuyFreeSpinSettingViewModel> GetById(int gameId)
            => mediator.Send(new GetBuyFreeSpinByGameIdRequest(gameId));

        [HttpPost]
        public Task<bool> Create(CreateBuyFreeSpinModel model)
            => mediator.Send(mapper.Map<CreateBuyFreeSpinRequest>(model));

        [HttpPut]
        public Task<bool> Update(UpdateBuyFreeSpinModel model)
            => mediator.Send(mapper.Map<UpdateBuyFreeSpinRequest>(model));

        [HttpPut("enable")]
        public Task<bool> Enable(EnableBuyFreeSpinModel model)
            => mediator.Send(mapper.Map<EnableBuyFreeSpinRequest>(model));
    }
}