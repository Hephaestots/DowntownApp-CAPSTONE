using Application.Common.Interfaces;
using Application.Core;
using Application.DTOs;
using Application.DTOs.Queries;
using Ardalis.GuardClauses;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Handlers.ChatRoomTypes.Queries
{
    /// <summary>
    /// Class that serves as the base for fetching a list of ChatRoomTypes.
    /// </summary>
    public class List
    {
        /// <summary>
        /// Class that serves as the Query for indicating the mediator action.
        /// </summary>
        public class Query : IRequest<Result<List<ChatRoomTypeDto>>> { }

        /// <summary>
        /// Class that serves as the Handler for the Query.
        /// </summary>
        public class Handler : IRequestHandler<Query, Result<List<ChatRoomTypeDto>>>
        {
            private readonly IDataContext _context;
            private readonly IMapper _mapper;

            public Handler(IDataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Result<List<ChatRoomTypeDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                Guard.Against.Null(_context.ChatRoomTypes, nameof(_context.ChatRoomTypes));

                var chatRoomTypes = await _context.ChatRoomTypes.ToListAsync(cancellationToken);
                var chatRoomTypeDto = _mapper.Map<List<ChatRoomTypeDto>>(chatRoomTypes);

                return Result<List<ChatRoomTypeDto>>.Success(chatRoomTypeDto);
            }
        }
    }
}
