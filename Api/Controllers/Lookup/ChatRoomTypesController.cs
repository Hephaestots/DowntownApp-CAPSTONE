using Api.Controllers.Base;
using Policies = Api.Constants.AuthorizationPolicyConstants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Application.Handlers.ChatRoomTypes.Queries;
using Application.Handlers.ChatRoomTypes.Commands;
using Application.DTOs;
using Application.DTOs.Commands;

namespace Api.Controllers.Lookup
{
    [Authorize(Policies.CREATOR)]
    [Route("api/[controller]")]
    public class ChatRoomTypesController : BaseApiController
    {
        public ChatRoomTypesController() { }

        #region Queries

        [HttpGet] //api/chatRoomtypes
        public async Task<IActionResult> GetChatRoomTypes()
        {
            return HandleResult(await Mediator.Send(new List.Query()));
        }

        [HttpGet("{id}")] //api/chatRoomtypes/{id}
        public async Task<IActionResult> GetChatRoomTypeDetails(Guid id)
        {
            return HandleResult(await Mediator.Send(new Details.Query { Id = id }));
        }

        #endregion

        #region Commands

        [HttpPost] //api/chatRoomtypes
        public async Task<IActionResult> CreateChatRoomType(ChatRoomTypeCommandDto chatRoomDto)
        {
            return HandleResult(await Mediator.Send(new Create.Command { ChatRoomType = chatRoomDto }));
        }

        [HttpPut("{id}")] //api/chatRoomtypes/{id}
        public async Task<IActionResult> UpdateChatRoomType(Guid id, ChatRoomTypeCommandDto chatRoomDto)
        {
            return HandleResult(await Mediator.Send(new Edit.Command { Id = id, ChatRoomType = chatRoomDto }));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteChatRoomType(Guid id)
        {
            return HandleResult(await Mediator.Send(new Delete.Command { Id = id }));
        }

        #endregion
    }
}
