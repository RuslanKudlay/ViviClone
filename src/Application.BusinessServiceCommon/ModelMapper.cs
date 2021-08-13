using Application.BBLInterfaces.BusinessServicesInterfaces;
using Application.EntitiesModels.Entities;
using Application.EntitiesModels.Entities.Chat;
using Application.EntitiesModels.Entities.Order;
using Application.EntitiesModels.Models;
using Application.EntitiesModels.Models.OrderModels;
using Application.EntitiesModels.Models.ChatModels;
using AutoMapper;
using System;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Linq;
using Application.EntitiesModels.Entities.WishList;

namespace Application.BusinessServiceCommon
{
    public class ModelMapper : IModelMapper
    {
        IMapper mapper;

        public ModelMapper()
        {
            Init();
        }

        public OUT MapTo<IN, OUT>(IN input)
        {
            return mapper.Map<OUT>(input);
        }

        public OUT MapTo<IN, OUT>(IN input, OUT output)
        {
            return mapper.Map(input, output);
        }

        private void Init()
        {
            var config = new MapperConfiguration(cfg =>
            {
                CreateMap(cfg);
                UserMap(cfg);
            });

            mapper = config.CreateMapper();
        }

        private void CreateMap(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<Order, OrderModel>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.OrderStatus.StatusName))
                .ForMember(dest => dest.DeliveryService, opt => opt.MapFrom(src => src.DeliveryService.Name));
            cfg.CreateMap<OrderDetails, OrderDetailsModel>();
            cfg.CreateMap<OrderHistory, OrderHistoryModel>();
            cfg.CreateMap<Ware, WareModel>()
                .ForMember(x => x.AverageRate, opt => opt.Ignore())
                .ForMember(dest => dest.CategoryValues, opt => opt.MapFrom(
                    src => src.WCV.Select(_ => _.CategoryValueses).Select(cv => new CategoryValuesModel()
                    {
                        Id = cv.Id,
                        Name = cv.Name,
                        IsEnable = cv.IsEnable,
                        CategoryId = cv.CategoryId,
                        CategoryName = cv.Category.Name
                    }).ToList()
                    ))
                .ForMember(dest => dest.GOWs, opt => opt.MapFrom(
                    src => src.WareGOWs.Select(w => w.GOW).Select(gw => new GOW()
                    { 
                        Id = gw.Id,
                        Name = gw.Name,
                        SubUrl = gw.SubUrl,
                        IsEnable = gw.IsEnable,
                        Parent = gw.Parent,
                        ParentId = gw.ParentId,
                        MetaDescription = gw.MetaDescription,
                        MetaKeywords = gw.MetaKeywords,
                        IconString = gw.IconString,
                        Childs = gw.Childs,
                        LogoImage = gw.LogoImage,
                        ShortDescription = gw.ShortDescription,
                    })
                    ));
            cfg.CreateMap<ChatModel, Chat>();
            cfg.CreateMap<GOW, GOWModel>();
            cfg.CreateMap<GOWModel, GOW>();
            cfg.CreateMap<Brand, BrandModel>();
            cfg.CreateMap<ChatMessageModel, ChatMessage>();
            cfg.CreateMap<ChatMessageModel, ChatMessage>();
            cfg.CreateMap<Slider, SliderModel>()
                .ForMember(x => x.Base64Image, opt => opt.Ignore())
                .ForMember(x => x.Location, opt => opt.Ignore())
                .ForMember(x => x.NameWare, opt => opt.Ignore());

            cfg.CreateMap<WishListWare, WishListWareModel>()
                .ForMember(x => x.WareId, opt => opt.MapFrom(src => src.WareId));
            cfg.CreateMap<WishList, WishListModel>()
                .ForMember(x => x.WishListWareModel, opt => opt.MapFrom(src => src.WishListWares));

            cfg.CreateMap<PostModel, Post>()
                .ForMember(x => x.Blog, opt => opt.Ignore())
                .ForMember(x => x.BlogId, opt => opt.Ignore());
            cfg.CreateMap<Post, PostModel>()
                .ForMember(x => x.Base64Image, opt => opt.Ignore());
        }

        private void UserMap(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<ApplicationUser, UserModel>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.UserRoles.Count != 0 ? src.UserRoles.ToList()[0].Role.Name : "Without role"));
        }
    }
}
