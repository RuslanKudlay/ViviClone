using System.Collections.Generic;

namespace Application.EntitiesModels.Models
{
    //GOW - Group of Ware
    public class GOWModel
    {
        public int Id { get; set; }
        public string Name { get; set; }        
        public GOWModel Parent { get; set; }
        public int? ParentId { get; set; }
        public string SubUrl { get; set; }
        public string MetaKeywords { get; set; }
        public string MetaDescription { get; set; }
        public string LogoImage { get; set; }
        public string IconString { get; set; }
        public bool IsEnable { get; set; }
        public string ShortDescription { get; set; }
        public List<GOWModel>  Childs { get; set; }
    }
}
