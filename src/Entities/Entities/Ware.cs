using Application.EntitiesModels.Entities.Order;
using System.Collections.Generic;

namespace Application.EntitiesModels.Entities
{
    public class Ware
    {
        public int Id { get; set; }

        public bool IsEnable { get; set; }

        public bool IsBestseller { get; set; }
        public bool IsOnlyForProfessionals { get; set; }

        public string VendorCode { get; set; }

        public string WareImage { get; set; }

        public string Name { get; set; }

        public string Text { get; set; }

        public double Price { get; set; }

        public string SubUrl { get; set; }

        public string MetaKeywords { get; set; }
        
        public string MetaDescription { get; set; }

        public virtual List<WaresCategoryValues> WCV { get; set; }

        public virtual List<WareVote> WareVote { get; set; }

        public virtual List<OrderDetails> OrderDetails { get; set; }

        public virtual List<WareGOW> WareGOWs { get; set; }

        public virtual List<WishList.WishListWare> WishListWares { get; set; }

        public int? BrandId { get; set; }

        public virtual Brand Brand { get; set; }

        public Ware()
        {

            WCV = new List<WaresCategoryValues>();

            WareVote = new List<WareVote>();
        }
    }
}