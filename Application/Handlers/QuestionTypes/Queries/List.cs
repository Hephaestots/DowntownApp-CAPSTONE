using Application.Common.Interfaces;
using Application.Core;
using Application.DTOs;
using Application.DTOs.Queries;
using Ardalis.GuardClauses;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Handlers.QuestionTypes.Queries
{
    /// <summary>
    /// Class that serves as the base for fetching a list of QuestionTypes.
    /// </summary>
    public class List
    {
        /// <summary>
        /// Class that serves as the Query for indicating the mediator action.
        /// </summary>
        public class Query : IRequest<Result<List<QuestionTypeDto>>> { }

        /// <summary>
        /// Class that serves as the Handler for the Query.
        /// </summary>
        public class Handler : IRequestHandler<Query, Result<List<QuestionTypeDto>>>
        {
            private readonly IDataContext _context;
            private readonly IMapper _mapper;

            public Handler(IDataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Result<List<QuestionTypeDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                Guard.Against.Null(_context.QuestionTypes, nameof(_context.QuestionTypes));

                var questions = await _context.QuestionTypes.ToListAsync(cancellationToken);
                var questionsDto = _mapper.Map<List<QuestionTypeDto>>(questions);

                return Result<List<QuestionTypeDto>>.Success(questionsDto);
            }
        }
    }
}
