using System;
using System.Collections.Generic;
using System.Text;

namespace Application.EntitiesModels.Models.QueryModels
{
    public class QueryModel<T>
    {
        public int? Skip { get; set; }
         
        public int? Take { get; set; }
         
        public string OrderBy { get; set; }
         
        public string OrderByDesc { get; set; }
         
        public DateTime? DateCreatedFrom { get; set; }
         
        public DateTime? DateCreatedTo { get; set; }

        public int TotalCount { get; set; }

        public int ResultCount { get; set; }
         
        public List<T> Result { get; set; }
    }
}
