using Application.EntitiesModels.Models.OrderModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.EntitiesModels.Models.QueryModels
{
    public class QueryOrderModel: QueryModel<OrderModel>
    {
        public int? UserId { get; set; }

        public string OrderNumberContains { get; set; }

        public string UserNameContains { get; set; }

        public string UserEmailContains { get; set; }       
    }
}
