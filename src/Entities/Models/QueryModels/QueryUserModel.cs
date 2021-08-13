using Application.EntitiesModels.Models.OrderModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.EntitiesModels.Models.QueryModels
{
    public class QueryUserModel : QueryModel<UserModel>
    {
        public string EmailContains { get; set; }       
    }
}
