using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using AutoMapper;
using WonderApp.Data;
using WonderApp.Models;

namespace WonderApp.Models.AutoMapperConfiguration
{
    public class AutoMapperConfigurator
    {
        public static void Configure()
        {
            Mapper.CreateMap<Deal, DealModel>();
            Mapper.CreateMap<DealModel, Deal>();

            Mapper.CreateMap<Category, CategoryModel>();
            Mapper.CreateMap<CategoryModel, Category>()
                .ForMember(dest => dest.Users, opt => opt.NullSubstitute(new List<AspNetUser>()));

            Mapper.CreateMap<Company, CompanyModel>();
            Mapper.CreateMap<CompanyModel, Company>();

            Mapper.CreateMap<Cost, CostModel>();
            Mapper.CreateMap<CostModel, Cost>();

            Mapper.CreateMap<Location, LocationModel>();
            Mapper.CreateMap<LocationModel, Location>();

            Mapper.CreateMap<Tag, TagModel>();
            Mapper.CreateMap<TagModel, Tag>();

            Mapper.CreateMap<AspNetUser, UserModel>();
            Mapper.CreateMap<UserModel, AspNetUser>();

            Mapper.CreateMap<AspNetUserLogin, AspNetUserLoginModel>();
            Mapper.CreateMap<AspNetUserLoginModel, AspNetUserLogin>();

            Mapper.CreateMap<Reminder, ReminderModel>();
            Mapper.CreateMap<ReminderModel, Reminder>();

            Mapper.CreateMap<Gender, GenderModel>();
            Mapper.CreateMap<GenderModel, Gender>();

            
        }

    }
}