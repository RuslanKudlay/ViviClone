using System;
using System.Collections.Generic;
using System.Text;

namespace Application.EntitiesModels.Models.OrderModels
{
    public class OrderHistoryModel
    {
        public Guid Id { get; set; }   

        public DateTime Date { get; set; }

        public string Message { get; set; }    
    }
}
