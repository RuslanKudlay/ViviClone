using Application.BBLInterfaces.BusinessServicesInterfaces;
using Application.EntitiesModels.Entities;
using Application.EntitiesModels.Models;
using Application.EntitiesModels.Models.ChatModels;
using Application.EntitiesModels.Models.QueryModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Api.Controllers
{
    [AllowAnonymous]
    public class ChatController : ApplicationApiController
    {
        IChatService chatService;

        public ChatController(IChatService chatService, UserManager<ApplicationUser> manager) : base(manager)
        {
            this.chatService = chatService;
        }

        [HttpPost]
        [Route("api/CloseChat/{id:int}")]
        public IActionResult Close(int id)
        {
            return InvokeMethod(chatService.CloseChat, id);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        [Route("api/GetChatByGuid/{chatGUID}")]
        public IActionResult GetChatByChatGuid(Guid chatGUID)
        {
            return InvokeMethod(chatService.GetChatByChatGuid, chatGUID);
        }

        [HttpPost]
        [Route("api/Chat/AddMessage")]
        public IActionResult AddMessage([FromBody]ChatMessageModel message)
        {
            return InvokeMethod(chatService.AddMessage, message);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        [Route("api/Chat/GetNew")]
        public IActionResult GetNew([FromBody]int[] alreadyReceived)
        {
            return InvokeMethod(chatService.GetNewestChat, alreadyReceived);
        }

        [HttpPost]
        [Route("api/Chat/NewestMsg")]
        public IActionResult GetNewestMsg([FromBody]NewestMsgQueryModel[] msgQuery)
        {
            return InvokeMethod(chatService.GetNewestMsg, msgQuery);
        }
    }
}
