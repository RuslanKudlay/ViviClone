using System;
using System.Collections.Generic;
using System.Text;

namespace Application.EntitiesModels.Entities
{
    public class WaresCategoryValues
    {
        public int Id { get; set; }

        public int WareId { get; set; }

        public int CategoryValueId { get; set; }

        public Ware Ware { get; set; }

        public CategoryValues CategoryValueses { get; set; }
        
    }
}
