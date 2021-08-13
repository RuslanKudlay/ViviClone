using System;
using System.Collections.Generic;
using System.Text;

namespace Application.EntitiesModels.Models.QueryModels
{
    public class QueryBrandModel : QueryModel<BrandModel>
    {
        public string NameContains { get; set; }

        public string BodyContains { get; set; }

        public string MetaKeywordsContains { get; set; }
    }
}
