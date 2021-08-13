using System;
using System.Collections.Generic;

namespace Application.EntitiesModels.Models.OrderModels
{
    public class OrderModel
    {
        public Guid Id { get; set; }
        public string OrderNumber { get; set; }
        public int CountOfWares { get; set; }
        public DateTime CreatedDate { get; set; }     
        public string Status { get; set; }
        public string DeliveryService { get; set; }
        public string DeclarationNumber { get; set; }

        public UserModel User { get; set; }
        public List<OrderHistoryModel> OrderHistories { get; set; }
        public List<OrderDetailsModel> OrderDetails { get; set; }

        public OrderModel()
        {
            OrderHistories = new List<OrderHistoryModel>();
            OrderDetails = new List<OrderDetailsModel>();
        }
    }
}
