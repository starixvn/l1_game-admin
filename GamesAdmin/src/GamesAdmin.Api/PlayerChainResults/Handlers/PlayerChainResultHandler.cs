using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Api.PlayerChainResults.Requests;
using GamesAdmin.Api.PlayerChainResults.ViewModels;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities;
using MediatR;
using MongoDB.Driver;

namespace GamesAdmin.Api.PlayerChainResults.Handlers
{
    public class PlayerChainResultHandler : IRequestHandler<GetPlayerChainResultsRequest, PlayerChainResultsResponse>
    {
        private readonly IGenericRepository<PlayerChainAssignmentEntity> repository;
        private readonly IMapper mapper;
        private static readonly TimeSpan GMT7Offset = TimeSpan.FromHours(7);

        public PlayerChainResultHandler(IGenericRepository<PlayerChainAssignmentEntity> repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<PlayerChainResultsResponse> Handle(GetPlayerChainResultsRequest request, CancellationToken cancellationToken)
        {
            var collection = repository.Collection();
            var filter = BuildFilter(request);

            var totalRecords = await collection.CountDocumentsAsync(filter, cancellationToken: cancellationToken);
            var results = await collection
                .Find(filter)
                .Sort(Builders<PlayerChainAssignmentEntity>.Sort.Descending(x => x.AssignedAt))
                .Skip((request.Page - 1) * request.PageSize)
                .Limit(request.PageSize)
                .ToListAsync(cancellationToken);

            return new PlayerChainResultsResponse
            {
                Results = mapper.Map<IEnumerable<PlayerChainResultViewModel>>(results),
                TotalRecords = (int)totalRecords,
                TotalPages = (int)Math.Ceiling((double)totalRecords / request.PageSize),
                CurrentPage = request.Page,
                PageSize = request.PageSize
            };
        }

        private static FilterDefinition<PlayerChainAssignmentEntity> BuildFilter(GetPlayerChainResultsRequest request)
        {
            var filterBuilder = Builders<PlayerChainAssignmentEntity>.Filter;
            
            // Convert GMT+7 input dates to UTC for database query
            var fromDateUTC = request.FromDate.HasValue 
                ? request.FromDate.Value.Subtract(GMT7Offset) 
                : (DateTime?)null;
            
            var toDateUTC = request.ToDate.HasValue 
                ? request.ToDate.Value.Date.AddDays(1).AddTicks(-1).Subtract(GMT7Offset) 
                : (DateTime?)null;
            
            var filterConditions = new (bool condition, Func<FilterDefinition<PlayerChainAssignmentEntity>> filter)[]
            {
                (!string.IsNullOrEmpty(request.GameId), () => filterBuilder.Eq(x => x.GameId, request.GameId)),
                (!string.IsNullOrEmpty(request.PlayerId), () => filterBuilder.Eq(x => x.PlayerId, request.PlayerId)),
                (fromDateUTC.HasValue, () => filterBuilder.Gte(x => x.AssignedAt, fromDateUTC.Value)),
                (toDateUTC.HasValue, () => filterBuilder.Lte(x => x.AssignedAt, toDateUTC.Value)),
                (request.ExcludeUUS, () => filterBuilder.Ne(x => x.Currency, "UUS"))
            };

            var filters = filterConditions
                .Where(x => x.condition)
                .Select(x => x.filter())
                .ToList();

            return filters.Any() ? filterBuilder.And(filters) : filterBuilder.Empty;
        }
    }
}
