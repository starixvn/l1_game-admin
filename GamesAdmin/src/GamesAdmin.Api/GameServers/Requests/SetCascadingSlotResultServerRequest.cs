using System.Collections.Generic;
using System.Linq;
using MediatR;

namespace GamesAdmin.Api.GameServers.Requests
{
    public class SetCascadingSlotResultServerRequest : IRequest<bool>
    {
        public SetCascadingSlotResultServerRequest(
            string name,
            IEnumerable<byte> baseSpin,
            IEnumerable<byte[]> tumblings)
        {
            Name = name;
            BaseSpin = baseSpin.ToList();
            Tumblings = tumblings.ToList();
        }

        public string Name { get; }

        public IEnumerable<byte> BaseSpin { get; }

        public IEnumerable<byte[]> Tumblings { get; }
    }
}