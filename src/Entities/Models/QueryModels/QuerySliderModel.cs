using System;
using System.Collections.Generic;
using System.Text;

namespace Application.EntitiesModels.Models.QueryModels
{
    public class QuerySliderModel: QueryModel<SliderModel>
    {
        public string NameWareContains { get; set; }
        public string TypeContains { get; set; }
    }
}
