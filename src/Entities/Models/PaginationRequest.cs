using System;
using System.Collections.Generic;
using System.Text;

namespace Application.EntitiesModels.Models
{
    public class PaginationRequest
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public Sort Sort { get; set; }
        public List<Filter> Filters { get; set; }
    }
    public class Sort
    {
        public string SortBy { get; set; }
        public SortDirection SortDirection { get; set; }
    }

    public class Filter
    {
        public string ColumnName { get; set; }
        public string Type { get; set; }
        public string FilterValue { get; set; }
        public string FilterType { get; set; }
        public string FilterValueTo { get; set; }
    }

    public enum SortDirection
    {
        Ascending,
        Descending
    }
}
