using Application.EntitiesModels.Models.ChatModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.BBLInterfaces.BusinessServicesInterfaces
{
    public interface IChatService
    {
        ChatModel CreateChat(string UserOrSessionId);

        ChatModel GetChatByChatGuid(Guid chatGUID);

        ChatModel GetChatById(int id);

        List<ChatModel> GetNewestChat(int[] alreadyReceived);

        ChatMessageModel AddMessage(ChatMessageModel message);

        List<ChatMessageModel> GetNewestMsg(NewestMsgQueryModel[] msgQueryModel);

        bool CloseChat(int id);
    }
}
