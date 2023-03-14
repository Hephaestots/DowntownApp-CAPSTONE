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

namespace Application.Handlers.QuestionTypes.Queries
{
    public class Details
    {
        public class Query : IRequest<Result<QuestionTypeDto?>>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<QuestionTypeDto?>>
        {
            private readonly IDataContext _context;
            private readonly IMapper _mapper;

            public Handler(IDataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Result<QuestionTypeDto?>> Handle(Query request, CancellationToken cancellationToken)
            {
                Guard.Against.Null(_context.QuestionTypes, nameof(_context.QuestionTypes));

                var questionDto = await _context.QuestionTypes
                                                .ProjectTo<QuestionTypeDto>(_mapper.ConfigurationProvider)
                                                .FirstOrDefaultAsync(ec => ec.Id == request.Id, cancellationToken);

                return Result<QuestionTypeDto?>.Success(questionDto);
            }
        }
    }
}
