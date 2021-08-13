using Application.EntitiesModels.Models.QueryModels;
using Application.EntitiesModels.Models.ViewsModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.EntitiesModels.Models
{
    public class BrandsCategoriesByWares
    {
        public QueryWaresModel Wares { get; set; }
        public SideSearchMenuModel SideSearchMenuModel { get; set; }
    }
}
