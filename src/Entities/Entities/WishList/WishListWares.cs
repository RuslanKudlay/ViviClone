using System;
using System.Collections.Generic;
using System.Text;

namespace Application.EntitiesModels.Entities.WishList
{
    public class WishListWare
    {
        public int Id { get; set; }
        public int WareId { get; set; }
        public virtual Ware Ware { get; set; }
        public int WishListId { get; set; }
        public WishList WishList { get; set; }
        public DateTime DateAdded { get; set; }
    }
}
