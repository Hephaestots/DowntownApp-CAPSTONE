using Api.Controllers.Base;
using Policies = Api.Constants.AuthorizationPolicyConstants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Application.Handlers.QuestionTypes.Queries;
using Application.Handlers.QuestionTypes.Commands;
using Application.DTOs;
using Application.DTOs.Commands;

namespace Api.Controllers.Lookup
{
    [Authorize(Policies.CREATOR)]
    [Route("api/[controller]")]
    public class QuestionTypesController : BaseApiController
    {
        public QuestionTypesController() { }

        #region Queries

        [HttpGet] //api/questionTypes
        public async Task<IActionResult> GetQuestionType()
        {
            return HandleResult(await Mediator.Send(new List.Query()));
        }

        [HttpGet("{id}")] //api/questiontypes/{id}
        public async Task<IActionResult> GetQuestionTypeDetails(Guid id)
        {
            return HandleResult(await Mediator.Send(new Details.Query { Id = id }));
        }

        #endregion

        #region Commands

        [HttpPost] //api/questiontypes
        public async Task<IActionResult> CreateQuestionType(QuestionTypeCommandDto questionDto)
        {
            return HandleResult(await Mediator.Send(new Create.Command { QuestionType = questionDto }));
        }

        [HttpPut("{id}")] //api/questiontypes/{id}
        public async Task<IActionResult> UpdateQuestionType(Guid id, QuestionTypeCommandDto questionDto)
        {
            return HandleResult(await Mediator.Send(new Edit.Command { Id = id, QuestionType = questionDto }));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuestionType(Guid id)
        {
            return HandleResult(await Mediator.Send(new Delete.Command { Id = id }));
        }

        #endregion
    }
}
