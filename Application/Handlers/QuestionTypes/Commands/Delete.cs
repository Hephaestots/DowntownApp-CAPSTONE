using Application.Common.Interfaces;
using Application.Core;
using Ardalis.GuardClauses;
using MediatR;

namespace Application.Handlers.QuestionTypes.Commands
{
    public class Delete
    {
        /// <summary>
        /// Command class used to start the request for deleting an QuestionType.
        /// </summary>
        public class Command : IRequest<Result<Unit>?>
        {
            public Guid Id { get; set; }
        }


        /// <summary>
        /// Handler class used to handle the deleting of the QuestionType.
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
                Guard.Against.Null(_context.QuestionTypes, nameof(_context.QuestionTypes));

                var questionType = await _context.QuestionTypes.FindAsync(new object?[] { request.Id }, cancellationToken);

                if (questionType is null) return null;

                _context.QuestionTypes.Remove(questionType);

                var result = await _context.SaveChangesAsync(cancellationToken) > 0;

                if (!result)
                    return Result<Unit>.Failure("Failed to delete a QuestionType.");
                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
