using System;
using System.Collections.Generic;
using System.Text;

namespace Application.EntitiesModels.Entities.Chat
{
    public class Chat
    {
        public int Id { get; set; }

        public DateTimeOffset CreatedDate { get; set; }

        public DateTimeOffset TimeToCloseChat { get; set; }

        public string SessionOrUserId { get; set; }

        public bool IsUserAuthorized { get; set; }

        public string UserNameOrEmail { get; set; }

        public bool IsChatEnded { get; set; }

        public Guid ChatGUID { get; set; }

        public virtual ICollection<ChatMessage> ChatMessages { get; set; }

        public Chat()
        {
            ChatMessages = new List<ChatMessage>();
        }
    }
}
