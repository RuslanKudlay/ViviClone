using System;
using System.Collections.Generic;
using System.Text;

namespace Application.EntitiesModels.Entities
{
    public class CategoryValues
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsEnable { get; set; }

        public int  CategoryId { get; set; }

        public virtual Category Category { get; set; }

        public List<WaresCategoryValues> WCV { get; set; }

        public CategoryValues()
        {
            WCV = new List<WaresCategoryValues>();
        }
    }
}
