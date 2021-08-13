using Application.EntitiesModels.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.EntitiesModels.Models
{
    public class SliderModel
    {
        public int Id { get; set; }
        public string Image { get; set; }      
        public string LinkToWare { get; set; }
        public TypeSlider Type { get; set; }

        /* Friendly View */
        public string Location {get; set;}
        public string NameWare {get; set;}
        public string Base64Image { get; set; }
    }
}
