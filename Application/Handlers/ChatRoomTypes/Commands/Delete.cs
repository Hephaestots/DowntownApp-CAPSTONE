using Application.Common.Interfaces;
using Application.Core;
using Ardalis.GuardClauses;
using MediatR;

namespace Application.Handlers.ChatRoomTypes.Commands
{
    public class Delete
    {
        /// <summary>
        /// Command class used to start the request for deleting a ChatRoomType.
        /// </summary>
        public class Command : IRequest<Result<Unit>?>
        {
            public Guid Id { get; set; }
        }


        /// <summary>
        /// Handler class used to handle the deleting of the ChatRoomType.
        /// </summary>
        public class Handler : IRequestHandler<Command, Result<Unit>?>
        {
            private readonly IDataContext _context;

            public Handler(IDataContext context)
            {
                _context = context;
            }


            /// <summary>
            /// Deleting logic handled.
            /// </summary>
            /// <param name="request">IRequest object, i.e. Delete.Command</param>
            /// <param name="cancellationToken"></param>
            /// <returns>Result<Unit></returns>
            public async Task<Result<Unit>?> Handle(Command request, CancellationToken cancellationToken)
            {
                Guard.Against.Null(_context.ChatRoomTypes, nameof(_context.ChatRoomTypes));

                var chatRoomType = await _context.ChatRoomTypes.FindAsync(new object?[] { request.Id }, cancellationToken);

                if (chatRoomType is null) return null;

                _context.ChatRoomTypes.Remove(chatRoomType);

                var result = await _context.SaveChangesAsync(cancellationToken) > 0;

                if (!result)
                    return Result<Unit>.Failure("Failed to delete a ChatRoomType.");
                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
