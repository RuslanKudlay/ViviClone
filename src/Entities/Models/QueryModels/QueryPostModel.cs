using System;
using System.Collections.Generic;
using System.Text;

namespace Application.EntitiesModels.Models.QueryModels
{
    public class QueryPostModel : QueryModel<PostModel>
    {
       public string TitleContains { get; set; }

       public string BodyContains { get; set; }

    }
}
