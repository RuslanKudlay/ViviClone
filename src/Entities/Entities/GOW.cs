using System.Collections.Generic;

namespace Application.EntitiesModels.Entities
{
    // GOW - Group of Wares
    public class GOW
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ParentId { get; set; }
        public string SubUrl { get; set; }
        public string MetaKeywords { get; set; }
        public string MetaDescription { get; set; }
        public bool IsEnable { get; set; }
        public string ShortDescription { get; set; }
        public string LogoImage { get; set; }
        public string IconString { get; set; }
        public virtual GOW Parent { get; set; }
        public virtual ICollection<GOW> Childs { get; set; }
        public virtual ICollection<WareGOW> WareGOWs { get; set; }
    }
}
