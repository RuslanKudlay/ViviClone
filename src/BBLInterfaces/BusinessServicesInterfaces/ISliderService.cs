using System;
using System.Collections.Generic;
using System.Text;
using Application.EntitiesModels.Models;
using Application.EntitiesModels.Models.QueryModels;

namespace Application.BBLInterfaces.BusinessServicesInterfaces
{
    public interface ISliderService
    {
        List<SliderModel> Get();
        QuerySliderModel GetAll(QuerySliderModel query);
        SliderModel GetById(int slideId);

        SliderModel AddSlide(SliderModel sliderModel);
        SliderModel UpdateSlide(SliderModel sliderModel);

        bool Delete(int id);
    }
}
