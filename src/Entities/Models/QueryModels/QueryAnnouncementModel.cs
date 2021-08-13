using System;
using System.Collections.Generic;
using System.Text;

namespace Application.EntitiesModels.Models.QueryModels
{
    public class QueryAnnouncementModel: QueryModel<AnnouncementModel>
    {
        public string TitleContains { get; set; }

        public string BodyContains { get; set; }

    }
}
