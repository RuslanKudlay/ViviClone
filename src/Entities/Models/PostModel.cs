using System;

namespace Application.EntitiesModels.Models
{
    public class PostModel
    {
        public int Id { get; set; }
        public bool IsEnable { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModificated { get; set; }
        public string SubUrl { get; set; }
        public string MetaKeywords { get; set; }
        public string MetaDescription { get; set; }
        public string ImageURL { get; set; }
        public string Base64Image { get; set; }
    }
}