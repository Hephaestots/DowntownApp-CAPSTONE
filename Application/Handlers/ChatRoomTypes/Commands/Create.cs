using Application.Common.Interfaces;
using Application.Core;
using Application.DTOs;
using Application.DTOs.Commands;
using Application.Validators;
using Ardalis.GuardClauses;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Handlers.ChatRoomTypes.Commands
{
    public class Create
    {
        /// <summary>
        /// Command class used to start the request for creating a new ChatRoomType.
        /// </summary>
        public class Command : IRequest<Result<Unit>>
        {
            public ChatRoomTypeCommandDto ChatRoomType { get; set; } = new ChatRoomTypeCommandDto();
        }

        /// <summary>
        /// Handler class used to handle the creation of the new ChatRoomType.
        /// </summary>
        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly ICurrentUserService _userService;
            private readonly IDataContext _context;
            private readonly IMapper _mapper;

            public Handler(ICurrentUserService userService, IDataContext context, IMapper mapper)
            {
                _userService = userService;
                _context = context;
                _mapper = mapper;
            }

            /// <summary>
            /// Creation logic handled.
            /// </summary>
            /// <param name="request">IRequest object, i.e. Create.Command</param>
            /// <param name="cancellationToken"></param>
            /// <returns>Result<Unit></returns>
            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                Guard.Against.Null(_context.ChatRoomTypes, nameof(_context.ChatRoomTypes));
                Guard.Against.Null(request.ChatRoomType, nameof(request.ChatRoomType));

                /// Make sure the creatorId is always populated. (FK_CONSTRAINT)
                var chatRoomType = _mapper.Map<ChatRoomType>(request.ChatRoomType);
                //chatRoomType.CreatorId = Guid.Parse(_userService.GetUserId()!);

                _context.ChatRoomTypes.Add(chatRoomType);

                bool result = await _context.SaveChangesAsync(cancellationToken) > 0;

                if (!result) 
                    return Result<Unit>.Failure("Failed to create a new ChatRoomType.");
                return Result<Unit>.Success(Unit.Value);
            }
        }

        /// <summary>
        /// Validator class used for synchronous validation during the process pipeline.
        /// </summary>
        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(c => c.ChatRoomType).SetValidator(new ChatRoomTypeCommandDtoValidator());
            }
        }
    }
}
