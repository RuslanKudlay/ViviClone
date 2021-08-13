using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Application.EntitiesModels.Entities.Chat
{
    public class ChatMessage
    {
        public int Id { get; set; }

        public string Message { get; set; }

        public bool IsMessageFromOperator  { get; set; }

        public DateTimeOffset CreatedDate { get; set; }

        public int ChatId { get; set; }

        [NotMapped]
        public bool EndedByOperator { get; set; }

        public virtual Chat Chat { get; set; }
    }
}
