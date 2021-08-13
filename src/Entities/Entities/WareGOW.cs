using System;
using System.Collections.Generic;
using System.Text;

namespace Application.EntitiesModels.Entities
{
    public class WareGOW
    {
        public int Id { get; set; }

        public int WareId { get; set; }

        public Ware Ware { get; set; }

        public int GOWId { get; set; }

        public GOW GOW { get; set; }
    }
}
