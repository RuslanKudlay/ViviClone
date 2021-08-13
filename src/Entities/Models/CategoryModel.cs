using System.Collections.Generic;

namespace Application.EntitiesModels.Models
{
    public class CategoryModel
    {
        public int Id { get; set; }

        public string Name { get; set; }
        
        public string SubUrl { get; set; }

        public string MetaKeywords { get; set; }

        public string MetaDescription { get; set; }     
        
        public bool IsEnable { get; set; }

        public int Index { get; set; }

        public List<CategoryValuesModel> CategoryValues { get; set; }
    }
}