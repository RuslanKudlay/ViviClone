using System;
using System.Collections.Generic;
using System.Text;

namespace Application.EntitiesModels.Entities
{
    public class WareVote
    {
        public int Id { get; set; }

        public int WareId { get; set; }

        public int UserId { get; set; }

        public int Rate { get; set; }

        public DateTime Date { get; set; }

        public Ware Ware { get; set; }
    }
}
