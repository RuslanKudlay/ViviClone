using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Application.EntitiesModels.Entities.Order
{
    public class Order
    {
        public Guid Id { get; set; }
        public string OrderNumber { get; set; }
        public int CountOfWares { get; set; }
        public DateTime CreatedDate { get; set; }
        public string DeclarationNumber {get; set; }

        public int? OrderStatusId { get; set; }
        public virtual OrderStatus OrderStatus { get; set; }

        public int? DeliveryServiceId { get; set; }
        public virtual DeliveryService DeliveryService { get; set; }

        public int UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        public virtual List<OrderHistory> OrderHistories { get; set; }
        public virtual List<OrderDetails> OrderDetails { get; set; }
    }
}
