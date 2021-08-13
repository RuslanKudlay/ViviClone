using System;

namespace Application.EntitiesModels.Entities.Order
{
    public class OrderDetails
    {
        public Guid Id { get; set; }
        public int Count { get; set; }

        public int WareId { get; set; }
        public virtual Ware Ware { get; set; }

        public Guid OrderId { get; set; }
        public virtual Order Order { get; set; }
    }
}
