using Newtonsoft.Json;
using System.Collections.Generic;

namespace Application.EntitiesModels.Models
{
    public class WareModel
    {
        public int Id { get; set; }
        public bool IsEnable { get; set; }
        public bool IsBestseller { get; set; }
        public bool IsOnlyForProfessionals { get; set; }
        public string VendorCode { get; set; }
        public string WareImage { get; set; }
        public string Base64Image { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public double Price { get; set; }
        public string SubUrl { get; set; }
        public string MetaKeywords { get; set; }
        public string MetaDescription { get; set; }
        public double AverageRate { get; set; }

        [JsonProperty("gows")]
        public List<GOWModel> GOWs { get; set; }
        public List<CategoryValuesModel> CategoryValues { get; set; }

        public int? BrandId { get; set; }
        public string BrandName { get; set; }
    }
}