using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.EntitiesModels.Models
{
    public class WishListWareModel
    {
        public int Id { get; set; }
        public int WareId { get; set; }
        public WareModel Ware { get; set; }
        public DateTime DateAdded { get; set; }
    }
}
