using System;
using System.Collections.Generic;
using System.Text;

namespace Application.EntitiesModels.Models.QueryModels
{
    public class QueryWaresModel : QueryModel<WareModel>
    {
        public string NameContains { get; set; }

        public string PriceContains { get; set; }

        public string VendoreCodeContains { get; set; }

        public string GroupOfWareContains { get; set; }

        public double MinPrice { get; set; }

        public double MaxPrice { get; set; }     
    }
}
