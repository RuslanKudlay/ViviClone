using System;

namespace Application.EntitiesModels.Entities.Order
{
    public class OrderHistory
    {
        public Guid Id { get; set; }
        public string Message { get; set; }
        public DateTime Date { get; set; }

        public Guid OrderId { get; set; }
        public virtual Order Order { get; set; }

        public int? UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}
