﻿using Application.Common.Interfaces;
using Application.Core;
using Application.DTOs;
using Ardalis.GuardClauses;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Handlers.Events.Queries
{
    public class Details
    {
        public class Query : IRequest<Result<EventDto?>>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<EventDto?>>
        {
            private readonly IDataContext _context;
            private readonly IMapper _mapper;

            public Handler(IDataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Result<EventDto?>> Handle(Query request, CancellationToken cancellationToken)
            {
                Guard.Against.Null(_context.Events, nameof(_context.Events));

                var @event = await _context.Events
                                           .ProjectTo<EventDto>(_mapper.ConfigurationProvider)
                                           .FirstOrDefaultAsync(edto => edto.Id == request.Id);

                return Result<EventDto?>.Success(@event);
            }
        }
    }
}
