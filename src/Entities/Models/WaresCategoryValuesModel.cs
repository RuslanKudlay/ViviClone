using System.Collections.Generic;

namespace Application.EntitiesModels.Models
{
    //WCV - WaresCategoryValues
    public class WaresCategoryValuesModel
    {
        public int Id { get; set; }       
        public WareModel Ware { get; set; }
        public List<CategoryValuesModel> CategoryValues { get; set; }
    }
}