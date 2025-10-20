using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Database.Entities;
using GamesAdmin.Database;
using GamesAdmin.Api.PlayerChainResults.Requests;
using GamesAdmin.Api.PlayerChainResults.ViewModels;
using MediatR;
using MongoDB.Driver;
using System;

namespace GamesAdmin.Api.PlayerChainResults.Handlers
{
    public class PlayerChainResultHandler : IRequestHandler<GetPlayerChainResultsRequest, PlayerChainResultsResponse>
    {
        private readonly IGenericRepository<PlayerChainAssignmentEntity> repository;
        private readonly IMapper mapper;

        public PlayerChainResultHandler(IGenericRepository<PlayerChainAssignmentEntity> repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<PlayerChainResultsResponse> Handle(GetPlayerChainResultsRequest request, CancellationToken cancellationToken)
        {
            var collection = repository.Collection();

            var filterBuilder = Builders<PlayerChainAssignmentEntity>.Filter;
            var filters = new List<FilterDefinition<PlayerChainAssignmentEntity>>();

            if (!string.IsNullOrEmpty(request.GameId))
            {
                filters.Add(filterBuilder.Eq(x => x.GameId, request.GameId));
            }

            if (!string.IsNullOrEmpty(request.PlayerId))
            {
                filters.Add(filterBuilder.Eq(x => x.PlayerId, request.PlayerId));
            }

            var filter = filters.Any() 
                ? filterBuilder.And(filters) 
                : filterBuilder.Empty;

            var totalRecords = await collection.CountDocumentsAsync(filter, cancellationToken: cancellationToken);
            var totalPages = (int)Math.Ceiling((double)totalRecords / request.PageSize);

            var sort = Builders<PlayerChainAssignmentEntity>.Sort.Descending(x => x.AssignedAt);

            var results = await collection
                .Find(filter)
                .Sort(sort)
                .Skip((request.Page - 1) * request.PageSize)
                .Limit(request.PageSize)
                .ToListAsync(cancellationToken);

            var mappedResults = mapper.Map<IEnumerable<PlayerChainResultViewModel>>(results);

            return new PlayerChainResultsResponse
            {
                Results = mappedResults,
                TotalRecords = (int)totalRecords,
                TotalPages = totalPages,
                CurrentPage = request.Page,
                PageSize = request.PageSize
            };
        }
    }
}
