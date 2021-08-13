using System;
using System.Collections.Generic;
using System.Text;

namespace Application.EntitiesModels.Models
{
    public class PaginationResultModel
    {
        public int TotalCount { get; set; }
        public object Result { get; set; }
    }
}
