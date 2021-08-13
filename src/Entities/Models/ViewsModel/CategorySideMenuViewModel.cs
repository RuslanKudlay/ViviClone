using System;
using System.Collections.Generic;
using System.Text;

namespace Application.EntitiesModels.Models.ViewsModel
{
    public class WareCategoryValuesModel
    {
        public CategoryModel Category { get; set; }
        public List<TotalCategoryValuesModel> CategoryValues { get; set; }
    }

    public class TotalCategoryValuesModel
    {
        public CategoryValuesModel CategoryValue { get; set; }
        public int Count { get; set; }
        public bool IsSelected { get; set; }
    }
}
