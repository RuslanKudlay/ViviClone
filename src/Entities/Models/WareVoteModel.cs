using System;
using System.Collections.Generic;
using System.Text;

namespace Application.EntitiesModels.Models
{
    public class WareVoteModel
    {
        public int Id { get; set; }

        public int WareId { get; set; }

        public int UserId { get; set; }

        public int Rate { get; set; }

        public DateTime Date { get; set; }

        public WareModel Ware { get; set; }
    }
}
