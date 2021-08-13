using System;
using System.Collections.Generic;
using System.Text;

namespace Application.EntitiesModels.Models.ViewsModel
{
    public class GowViewModel
    {
        public GOWModel Parent { get; set; }

        public List<GOWModel> Child { get; set; }
    }
}
