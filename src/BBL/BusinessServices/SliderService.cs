using Application.BBLInterfaces.BusinessServicesInterfaces;
using Application.DAL;
using Application.EntitiesModels.Entities;
using Application.EntitiesModels.Models;
using Application.EntitiesModels.Models.QueryModels;
using Application.EntitiesModels.Models.ViewsModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Application.BBL.BusinessServices
{
    public class SliderService : ISliderService
    {
        private readonly IDbContextFactory _dbContextFactory;
        private readonly IPictureAttacherService _pictureAttacherService;
        private readonly IWareService _wareService;
        private readonly ILogger<SliderService> _logger;

        private readonly string ADDITION_TO_URL;

        public SliderService(
            IDbContextFactory dbContextFactory, 
            IPictureAttacherService pictureAttacherService, 
            IWareService wareService, 
            ILogger<SliderService> logger,
            IHostingEnvironment env)
        {
            _dbContextFactory = dbContextFactory;
            _pictureAttacherService = pictureAttacherService;
            _wareService = wareService;
            _logger = logger;

            if(env.IsDevelopment())
            {
                ADDITION_TO_URL = "http://localhost:5001/Shop/WareDetails?subUrl=";
            } else
            {
                ADDITION_TO_URL = "http://212.3.101.119:5160/Shop/WareDetails?subUrl=";
            }
        }

        public SliderModel AddSlide(SliderModel sliderModel)
        {
            using (var context = _dbContextFactory.Create())
            {
                var slide = new Slider();
                var isAdding = true;

                if (sliderModel.Id > 0)
                {
                    slide = context.Sliders.Where(a => a.Id == sliderModel.Id).FirstOrDefault();
                    if (slide == null)
                    {
                        _logger.Log(LogLevel.Error, new EventId(LoggerId.Error), "Slide was not found! Slide wasn't be added");
                    }
                    isAdding = false;
                }

                slide.Image = sliderModel.Image;
                slide.LinkToWare = sliderModel.LinkToWare;
                slide.Type = (EntitiesModels.Entities.TypeSlider)sliderModel.Type;

                if (isAdding)
                {
                    context.Sliders.Add(slide);
                }

                context.SaveChanges();

                return sliderModel;
            }
        }

        public List<SliderModel> Get()
        {
            using (var context = _dbContextFactory.Create())
            {
                List<SliderModel> sliderModels = context.Sliders.ToList().Select((s) => new SliderModel
                {
                    Id = s.Id,
                    Image = s.Image,
                    LinkToWare = s.LinkToWare,
                    Type = s.Type

                }).ToList();

                foreach (var slider in sliderModels)
                {
                    // LogoImage is like '/api/getImage/97' string
                    if (!string.IsNullOrWhiteSpace(slider.Image))
                    {
                        var imageId = int.Parse(slider.Image.Split('/')[3]);
                        var image = _pictureAttacherService.GetPictureData(imageId);
                        var base64image = image != null ? string.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(image, 0, image.Length)) : "";

                        slider.Base64Image = base64image;
                    }
                }

                return sliderModels;
            }
        }

        public QuerySliderModel GetAll(QuerySliderModel queryModel)
        {
            using (var context = _dbContextFactory.Create())
            {
                var resultSliderModel = new QuerySliderModel();

                List<SliderModel> sliderModels = context.Sliders.ToList().Select((s) => new SliderModel
                {
                    Id = s.Id,
                    Image = s.Image,
                    LinkToWare = s.LinkToWare,
                    Type = s.Type,
                    Location = Enum.GetName(typeof(TypeSlider), s.Type)
                }).ToList();

                foreach (var slider in sliderModels)
                {
                    // LogoImage is like '/api/getImage/97' string
                    if (!string.IsNullOrWhiteSpace(slider.Image))
                    {
                        var imageId = int.Parse(slider.Image.Split('/')[3]);
                        var image = _pictureAttacherService.GetPictureData(imageId);
                        var base64image = image != null ? string.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(image, 0, image.Length)) : "";

                        slider.Base64Image = base64image;
                    }

                    string wareUrl = slider.LinkToWare.Replace(ADDITION_TO_URL, "");
                    slider.NameWare = _wareService.GetWareBySubUrl(wareUrl).Name;
                }

                if (!string.IsNullOrEmpty(queryModel.NameWareContains))
                    sliderModels = sliderModels.Where(x => x.NameWare.Contains(queryModel.NameWareContains)).ToList();

                if (!string.IsNullOrEmpty(queryModel.TypeContains))
                    sliderModels = sliderModels.Where(x => x.Location.Contains(queryModel.TypeContains)).ToList();

                if (!string.IsNullOrEmpty(queryModel.OrderBy))
                {
                    if (queryModel.OrderBy.Contains("nameWare"))
                        sliderModels = sliderModels.OrderBy(x => x.NameWare).ToList();
                }

                if (!string.IsNullOrEmpty(queryModel.OrderByDesc))
                {
                    if (queryModel.OrderBy.Contains("nameWare"))
                        sliderModels = sliderModels.OrderByDescending(x => x.NameWare).ToList();
                }

                resultSliderModel.TotalCount = sliderModels.Count;

                if (queryModel.Skip != null) sliderModels = sliderModels.Skip((int)queryModel.Skip).ToList();

                if (queryModel.Take != null) sliderModels = sliderModels.Take((int)queryModel.Take).ToList();

                resultSliderModel.Result = sliderModels;

                return resultSliderModel;
            }
        }

        public SliderModel GetById(int slideId)
        {
            using (var context = _dbContextFactory.Create())
            {
                SliderModel sliderModel = context.Sliders.ToList()
                    .Where(w => w.Id == slideId)
                    .Select((s) => new SliderModel
                    {
                        Id = s.Id,
                        Image = s.Image,
                        LinkToWare = s.LinkToWare,
                        Type = s.Type

                    }).FirstOrDefault();

                // LogoImage is like '/api/getImage/97' string
                if (!string.IsNullOrWhiteSpace(sliderModel.Image))
                {
                    var imageId = int.Parse(sliderModel.Image.Split('/')[3]);
                    var image = _pictureAttacherService.GetPictureData(imageId);
                    var base64image = image != null ? string.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(image, 0, image.Length)) : "";

                    sliderModel.Base64Image = base64image;
                }

                return sliderModel;
            }          
        }

        public SliderModel UpdateSlide(SliderModel sliderModel)
        {
            using (var context = _dbContextFactory.Create())
            {
                var slide = context.Sliders.Where(a => a.Id == sliderModel.Id).FirstOrDefault();

                if (slide == null)
                {
                    _logger.Log(LogLevel.Error, new EventId(LoggerId.Error), "Slide was not found! Slide wasn't be updated");
                }

                slide.Image = sliderModel.Image;
                slide.LinkToWare = sliderModel.LinkToWare;
                slide.Type = (EntitiesModels.Entities.TypeSlider)sliderModel.Type;

                context.SaveChanges();

                return sliderModel;
            }
        }

        public bool Delete(int id)
        {
            using (var context = _dbContextFactory.Create())
            {
                var slide = context.Sliders.FirstOrDefault(s => s.Id == id);

                if (slide == null)
                {
                    _logger.Log(LogLevel.Error, new EventId(LoggerId.Error), "Slide was not found! Slide wasn't be deleted");
                }

                context.Sliders.Remove(slide);

                context.SaveChanges();

                return true;
            }
        }
    }
}
