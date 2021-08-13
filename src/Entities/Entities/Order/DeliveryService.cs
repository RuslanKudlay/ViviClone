using System.Collections.Generic;

namespace Application.EntitiesModels.Entities.Order
{
    public class DeliveryService
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual List<Order> Orders { get; set; }
    }
}
