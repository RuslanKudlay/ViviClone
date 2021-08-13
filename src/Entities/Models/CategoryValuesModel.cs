using System.Collections.Generic;

namespace Application.EntitiesModels.Models
{
    public class CategoryValuesModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string CategoryName { get; set; }

        public int CategoryId { get; set; }      
        
        public CategoryModel Category { get; set; }
        
        public bool IsEnable { get; set; }

        public bool IsDisabled { get; set; }
    }
}