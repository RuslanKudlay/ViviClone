using System;
using System.Collections.Generic;
using System.Text;

namespace Application.EntitiesModels.Entities
{
    public class Slider
    {
        public int Id { get; set; }
        public string Image { get; set; }
        public string LinkToWare { get; set; }
        public TypeSlider Type { get; set; }
    }

    public enum TypeSlider: int
    {
        Main = 1,
        Shop
    }
}
