using Application.Common.Interfaces;
using Application.Core;
using Application.DTOs;
using Application.DTOs.Queries;
using Ardalis.GuardClauses;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Handlers.ChatRoomTypes.Queries
{
    public class Details
    {
        public class Query : IRequest<Result<ChatRoomTypeDto?>>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<ChatRoomTypeDto?>>
        {
            private readonly IDataContext _context;
            private readonly IMapper _mapper;

            public Handler(IDataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Result<ChatRoomTypeDto?>> Handle(Query request, CancellationToken cancellationToken)
            {
                Guard.Against.Null(_context.ChatRoomTypes, nameof(_context.ChatRoomTypes));

                var chatRoomDto = await _context.ChatRoomTypes
                                                .ProjectTo<ChatRoomTypeDto>(_mapper.ConfigurationProvider)
                                                .FirstOrDefaultAsync(ec => ec.Id == request.Id, cancellationToken);

                return Result<ChatRoomTypeDto?>.Success(chatRoomDto);
            }
        }
    }
}
