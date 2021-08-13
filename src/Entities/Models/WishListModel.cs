using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.EntitiesModels.Models
{
    public class WishListModel
    {
        public int Id { get; set; }
        public UserModel User { get; set; }
        public string Name { get; set; }
        public double TotalPrice { get; set; }
        public List<WishListWareModel> WishListWareModel { get; set; }
    }
}
