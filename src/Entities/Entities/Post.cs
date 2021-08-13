using System;

namespace Application.EntitiesModels.Entities
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModificated { get; set; }
        public string SubUrl { get; set; }
        public string MetaKeywords { get; set; }
        public string MetaDescription { get; set; }
        public int BlogId { get; set; }
        public virtual Blog Blog { get; set; }
        public string ImageURL { get; set; }
        public bool IsEnable { get; set; }
    }
}
