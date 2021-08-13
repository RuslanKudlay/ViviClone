using System;
using System.Collections.Generic;
using System.Text;

namespace Application.EntitiesModels.Models.ViewsModel
{
    public class BasketModel
    {
        public List<BasketItemModel> BasketItems { get; set; }

        public double TotalPrice { get; set; }

        public BasketModel()
        {
            BasketItems = new List<BasketItemModel>();
        }

        public bool IsFullDiplay { get; set; }
    }

    public class BasketItemModel
    {
        public WareModel Ware { get; set; }
        public int WareQuantity { get; set; }
    }
}
