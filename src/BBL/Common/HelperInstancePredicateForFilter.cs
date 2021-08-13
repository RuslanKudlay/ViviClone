using Application.EntitiesModels.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Application.BBL.Common
{
    public class HelperInstancePredicateForFilter
    {
        public static Expression<Func<WareModel, bool>> GetPredicate(PaginationRequest paging)
        {
            var predicate = PredicateBuilder.True<WareModel>();
            foreach (var filter in paging.Filters)
            {
                switch (filter.ColumnName)
                {
                    case "name":
                        predicate = predicate.And(p => p.Name.ToLower().Contains(filter.FilterValue.ToLower()));
                        break;
                    case "vendorCode":
                        predicate = predicate.And(p => p.VendorCode.ToLower().Contains(filter.FilterValue.ToLower()));
                        break;
                    default:
                        break;
                }
            }
            return predicate;
        }
    }
}
