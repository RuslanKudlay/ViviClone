using System;
using System.Collections.Generic;
using System.Text;

namespace Application.EntitiesModels.Entities.WishList
{
    public class WishList
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public string Name { get; set; }
        public virtual List<WishListWare> WishListWares { get; set; }
    }
}
