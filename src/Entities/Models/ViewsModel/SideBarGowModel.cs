using System;
using System.Collections.Generic;
using System.Text;

namespace Application.EntitiesModels.Models.ViewsModel
{
    public class SideBarGowModel
    {
        public List<GOWModel> GroupOfWares { get; set; }

        public bool isСollapsedMenu { get; set; }
    }
}
