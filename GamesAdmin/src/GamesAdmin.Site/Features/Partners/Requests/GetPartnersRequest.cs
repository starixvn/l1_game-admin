using System.Collections.Generic;
using GamesAdmin.Core.Models.Partners;
using MediatR;

namespace GamesAdmin.Site.Features.Partners.Requests
{
  public class GetPartnersRequest : IRequest<IEnumerable<Partner>>
  {
  }
}