using System.Collections.Generic;
using GamesAdmin.Site.Features.Report.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.ResultMonitor.Requests
{
    public class SetHiLoResultRequest : IRequest<IEnumerable<CardViewModel>>
    {
        public SetHiLoResultRequest(string nickname, IEnumerable<string> result)
        {
            Name = nickname;
            Result = result;
        }

        public string Name { get; }

        public IEnumerable<string> Result { get; }
    }
}