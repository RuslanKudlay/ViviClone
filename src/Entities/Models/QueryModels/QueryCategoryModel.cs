using System;
using System.Collections.Generic;
using System.Text;

namespace Application.EntitiesModels.Models.QueryModels
{
    public class QueryCategoryModel: QueryModel<CategoryModel>
    {
        public string NameContains { get; set; }
    }
}
