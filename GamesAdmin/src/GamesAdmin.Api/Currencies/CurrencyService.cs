using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Api.Currencies.Requests;
using GamesAdmin.Core.Models.Currencies;
using MediatR;

namespace GamesAdmin.Api.Currencies
{
    public interface ICurrencyService
    {
        Task<IEnumerable<CurrencyModel>> GetAll();

        Task<CurrencyModel> GetById(string id);

        Task<IEnumerable<CurrencyModel>> GetByPartner(string partnerId);

        Task<IEnumerable<CurrencyModel>> GetByGroup(string groupId);

        Task<bool> ChangeCurrencyGroup(string id, string groupId);
    }

    public class CurrencyService : ICurrencyService
    {
        private readonly IMediator mediator;
        private readonly IMapper mapper;

        public CurrencyService(IMediator mediator, IMapper mapper)
        {
            this.mediator = mediator;
            this.mapper = mapper;
        }

        public async Task<bool> ChangeCurrencyGroup(string id, string groupId)
        {
            var currency = await GetById(id);

            if (currency != null)
            {
                var updateModel = mapper.Map<UpdateCurrencyModel>(currency);

                updateModel.GroupId = groupId;

                return await mediator.Send(new UpdateRequest(updateModel));
            }

            return false;
        }

        public Task<IEnumerable<CurrencyModel>> GetAll()
            => mediator.Send(new GetAllRequest());

        public Task<IEnumerable<CurrencyModel>> GetByGroup(string groupId)
            => mediator.Send(new GetByGroupRequest(new GetByGroupModel(groupId)));

        public Task<CurrencyModel> GetById(string id)
            => mediator.Send(new GetByIdRequest(id));

        public Task<IEnumerable<CurrencyModel>> GetByPartner(string partnerId)
            => mediator.Send(new GetByPartnerRequest(new GetByPartnerModel(partnerId)));
    }
}