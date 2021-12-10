using MediatR;

namespace GamesAdmin.Api.Currencies.Requests
{
    public class Request<TModel, TResponse> : IRequest<TResponse>
    {
        public Request(TModel model)
        {
            Model = model;
        }

        public TModel Model { get; }
    }
}