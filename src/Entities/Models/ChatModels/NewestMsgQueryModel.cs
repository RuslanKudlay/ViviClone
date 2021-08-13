using System;
using System.Collections.Generic;
using System.Text;

namespace Application.EntitiesModels.Models.ChatModels
{
    public class NewestMsgQueryModel
    {
        public int ChatId { get; set; }

        public DateTime LastMsgDate { get; set; }
    }
}
