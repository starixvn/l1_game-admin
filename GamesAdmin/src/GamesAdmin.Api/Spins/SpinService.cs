using GamesAdmin.Api.Spins.Requests;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities.Spins;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GamesAdmin.Api.Spins
{
    public interface ISpinService
    {
        Task<bool> AddSpin(CreateRequest createRequest);
    }

    public class SpinService : ISpinService
    {
        private readonly IGenericRepository<SpinEntity> spinRepository;

        public SpinService(IGenericRepository<SpinEntity> spinRepository)
        {
            this.spinRepository = spinRepository;
        }

        public async Task<bool> AddSpin(CreateRequest createRequest)
        {
            var spins = new List<SpinEntity>();
            var time = DateTime.UtcNow;

            for (var i = 0; i < createRequest.Spins; i++)
            {
                spins.Add(new SpinEntity
                {
                    ObCustId = createRequest.ObCustId,
                    SiteId = createRequest.SiteId,
                    Time = time
                });
            }

            await spinRepository.InsertManyAsync(spins);

            return true;
        }
    }
}
