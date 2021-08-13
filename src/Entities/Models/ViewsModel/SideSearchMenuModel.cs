using System;
using System.Collections.Generic;
using System.Text;

namespace Application.EntitiesModels.Models.ViewsModel
{
    public class SideSearchMenuModel
    {
        public IEnumerable<TotalBrandsModel> Brands { get; set; }
        public IEnumerable<WareCategoryValuesModel> WareCategoryValues { get; set; }
        public Professional Professional { get; set; }
    }

    public class TotalBrandsModel
    {
        public BrandModel Brand { get; set; }
        public int Count { get; set; }
        public bool IsSelected { get; set; }
        public bool IsDisabled { get; set; }
    }

    public class Professional
    {
        public string Name { get; set; } = "Professional";
        public bool IsSelected { get; set; }
        public bool IsDisabled { get; set; }
        public int Count { get; set; }
    }
}
