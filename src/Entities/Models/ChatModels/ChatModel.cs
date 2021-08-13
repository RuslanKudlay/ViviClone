using System;
using System.Collections.Generic;
using System.Text;

namespace Application.EntitiesModels.Models.ChatModels
{
    public class ChatModel
    {
        public int Id { get; set; }

        public DateTimeOffset CreatedDate { get; set; }

        public DateTimeOffset TimeToCloseChat { get; set; }

        public string SessionOrUserId { get; set; }

        public bool IsUserAuthorized { get; set; }

        public string UserNameOrEmail { get; set; }

        public bool IsChatEnded { get; set; }

        public Guid ChatGUID { get; set; }

        public List<ChatMessageModel> ChatMessages { get; set; }

        public ChatModel()
        {
            ChatMessages = new List<ChatMessageModel>();
        }
    }
}
