using System.Collections.Generic;

namespace Application.EntitiesModels.Entities
{
    public class Category
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string SubUrl { get; set; }

        public string MetaKeywords { get; set; }

        public string MetaDescription { get; set; }   
        
        public bool IsEnable { get; set; }
        public int Index { get; set; }

        public virtual ICollection<CategoryValues> CategoryValueses { get; set; }

        public Category()
        {
            CategoryValueses = new List<CategoryValues>();
        }
    }
}