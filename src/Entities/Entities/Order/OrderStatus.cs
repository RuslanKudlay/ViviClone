using System;
using System.Collections.Generic;

namespace Application.EntitiesModels.Entities.Order
{
    public class OrderStatus
    {
        public int Id { get; set; }
        public string StatusName { get; set; }

        public virtual List<Order> Orders { get; set; }
    }
}
