using System.Collections.Generic;

namespace Application.EntitiesModels.Entities
{
    public class Brand
    {
        public int Id { get; set; }
        public bool IsEnable { get; set; }
        public int Position { get; set; }
        public string Color { get; set; }
        public string ColorTitle { get; set; }
        public string LogoImage { get; set; }
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string Body { get; set; }
        public string SubUrl { get; set; }
        public string MetaKeywords { get; set; }
        public string MetaDescription { get; set; }      
        public virtual IList<Ware> Wares { get; set; }
    }
}