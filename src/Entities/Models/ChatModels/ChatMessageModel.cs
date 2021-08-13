using System;
using System.Collections.Generic;
using System.Text;

namespace Application.EntitiesModels.Models.ChatModels
{
     public class ChatMessageModel
    {
        public int Id { get; set; }

        public int ChatId { get; set; }

        public string Message { get; set; }

        public bool IsMessageFromOperator { get; set; }

        public bool EndedByOperator { get; set; }

        public DateTimeOffset? CreatedDate { get; set; }
    }
}
