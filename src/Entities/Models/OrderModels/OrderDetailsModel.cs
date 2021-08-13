using System;
using System.Collections.Generic;
using System.Text;

namespace Application.EntitiesModels.Models.OrderModels
{
    public class OrderDetailsModel
    {
        public Guid Id { get; set; }       

        public int Count { get; set; }

        public WareModel Ware { get; set; }      
    }
}
