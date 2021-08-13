using System.Collections.Generic;

namespace Application.EntitiesModels.Models
{
    public class NewsModel
    {
        public IList<PromotionModel> Promotions { get; set; }
        
        public IList<AnnouncementModel> Announcements { get; set; }
    }
}