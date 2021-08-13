using System;
using System.Collections.Generic;

namespace Application.EntitiesModels.Entities
{
    public class Blog
    {
        public int Id { get; set; }

        public bool IsEnable { get; set; }

        public string Title { get; set; }

        public string Body { get; set; }

        public DateTime? DateModificated { get; set; }

        public string SubUrl { get; set; }

        public string MetaKeywords { get; set; }

        public string MetaDescription { get; set; }

        public virtual ICollection<Post> Posts { get; set; }

        public Blog()
        {
            Posts = new List<Post>();
        }
    }
}