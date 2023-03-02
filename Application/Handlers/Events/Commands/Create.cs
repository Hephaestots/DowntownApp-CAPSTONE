﻿using Application.Common.Interfaces;
using Application.Core;
using Application.DTOs;
using Application.Validators;
using Ardalis.GuardClauses;
using AutoMapper;
using Domain.Entities;
using Domain.Events.Events;
using FluentValidation;
using MediatR;

namespace Application.Handlers.Events.Commands
{
    public class Create
    {
        /// <summary>
        /// Command class used to start the request for creating a new Event.
        /// </summary>
        public class Command : IRequest<Result<Unit>>
        {
            public EventDto Event { get; set; } = new EventDto();
        }

        /// <summary>
        /// Handler class used to handle the creation of the new Event.
        /// </summary>
        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly IDataContext _dataContext;
            private readonly IMapper _mapper;
            private readonly ICurrentUserService _userService;

            public Handler(IDataContext dataContext, IMapper mapper, ICurrentUserService userService)
            {
                _dataContext = dataContext;
                _mapper = mapper;
                _userService = userService;
            }

            /// <summary>
            /// Creation logic handled.
            /// </summary>
            /// <param name="request">IRequest object, i.e. Create.Command</param>
            /// <param name="cancellationToken"></param>
            /// <returns>Result<Unit></returns>
            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                Guard.Against.Null(_dataContext.Events, nameof(_dataContext.Events));

                /// Make sure the creatorId is always populated. (FK_CONSTRAINT)
                request.Event.CreatorId ??= Guid.Parse(_userService.GetUserId()!);

                var @event = _mapper.Map<Event>(request.Event);

                /// Add a domain event to the entity so our INotification handler can perform some work.
                @event.AddDomainEvent(new CreatedEvent(@event));

                _dataContext.Events.Add(@event);

                bool result = await _dataContext.SaveChangesAsync(cancellationToken) > 0;

                if (!result)
                    return Result<Unit>.Failure("Failed to create a new Event.");
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
                RuleFor(x => x.Event).SetValidator(new EventDtoValidator());
            }
        }
    }
}
