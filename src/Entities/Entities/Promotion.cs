 using System;

namespace Application.EntitiesModels.Entities
{
    public class Promotion
    {
        public int Id { get; set; }

        public bool IsEnable { get; set; }

        public string Title { get; set; }

        public string Body { get; set; }

        public DateTime Date { get; set; }

        public DateTime LastUpdateDate { get; set; }

        public string SubUrl { get; set; }

        public string MetaKeywords { get; set; }
        
        public string MetaDescription { get; set; }
    }
}