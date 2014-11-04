﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using AutoMapper;
using WonderApp.Data;
using WonderApp.Models;
using WonderApp.Models.Extensions;
using System.Data.Entity.Spatial;
using WonderApp.Models.Helpers;

namespace WonderApp.Models.AutoMapperConfiguration
{
    public class AutoMapperConfigurator
    {
        public static void Configure()
        {
            Mapper.CreateMap<Deal, DealModel>()
                .ForMember(m => m.ExpiryDate, opt => opt.MapFrom(e => e.AlwaysAvailable != null && e.AlwaysAvailable.Value ? String.Empty : e.ExpiryDate.MapToString()));
            Mapper.CreateMap<DealModel, Deal>()
                .ForMember(e => e.Id, opt => opt.Ignore())
                .ForMember(e => e.MyRejectUsers, opt => opt.Ignore())
                .ForMember(e => e.MyWonderUsers, opt => opt.Ignore())
                .ForMember(e => e.Company, opt => opt.Ignore())
                .ForMember(e => e.Tags, opt => opt.Ignore())
                .ForMember(e => e.Cost, opt => opt.Ignore())
                .ForMember(e => e.Category, opt => opt.Ignore())
                .ForMember(e => e.Images, opt => opt.Ignore())
                .ForMember(e => e.City, opt => opt.Ignore())
                .ForMember(e => e.AspNetUser, opt => opt.Ignore())
                .ForMember(e => e.ExpiryDate, opt => opt.MapFrom(m => m.AlwaysAvailable ? DateTime.Now.ToShortDateString() : m.ExpiryDate))
                .ForAllMembers(opt => opt.Condition(srs => !srs.IsSourceValueNull));

            Mapper.CreateMap<Deal, DealSummaryModel>();
                //.ForMember(m => m.ExpiryDate, opt => opt.ResolveUsing(e => e.ExpiryDate.MapToString()));

            Mapper.CreateMap<Category, CategoryModel>()
                .ForMember(dest => dest.Users, opt => opt.NullSubstitute(new List<UserModel>()));
            Mapper.CreateMap<CategoryModel, Category>()
                .ForMember(e => e.Deals, opt => opt.Ignore())
                .ForMember(e => e.Users, opt => opt.Ignore());

            Mapper.CreateMap<Company, CompanyModel>();
            Mapper.CreateMap<CompanyModel, Company>()
                .ForMember(e => e.Deals, opt => opt.Ignore());

            Mapper.CreateMap<Cost, CostModel>();
            Mapper.CreateMap<CostModel, Cost>()
                .ForMember(e => e.Deals, opt => opt.Ignore());

            Mapper.CreateMap<Location, LocationModel>()
                .ForMember(m => m.Latitude, opt => opt.MapFrom(e => e.Geography.Latitude))
                .ForMember(m => m.Longitude, opt => opt.MapFrom(e => e.Geography.Longitude));

            Mapper.CreateMap<LocationModel, Location>()
                .ForMember(e => e.Geography, opt => opt.MapFrom(m => 
                    GeographyHelper.ConvertLatLonToDbGeography(m.Longitude.Value, m.Latitude.Value)))
                .ForMember(e => e.Id, opt => opt.Ignore())
                .ForMember(e => e.Deals, opt => opt.Ignore())
                .ForMember(e => e.AspNetUser, opt => opt.Ignore());

            Mapper.CreateMap<Tag, TagModel>();
            Mapper.CreateMap<TagModel, Tag>()
                .ForMember(e => e.Deals, opt => opt.Ignore());

            Mapper.CreateMap<AspNetUser, UserModel>();
            Mapper.CreateMap<UserModel, AspNetUser>();

            Mapper.CreateMap<AspNetUserLogin, AspNetUserLoginModel>();
            Mapper.CreateMap<AspNetUserLoginModel, AspNetUserLogin>();

            Mapper.CreateMap<Reminder, ReminderModel>();
            Mapper.CreateMap<ReminderModel, Reminder>();

            Mapper.CreateMap<Gender, GenderModel>();
            Mapper.CreateMap<GenderModel, Gender>();

            Mapper.CreateMap<Image, ImageModel>();
            Mapper.CreateMap<ImageModel, Image>();

            Mapper.CreateMap<Device, DeviceModel>();
            Mapper.CreateMap<DeviceModel, Device>();

            Mapper.CreateMap<City, CityModel>();
            Mapper.CreateMap<CityModel, City>();

            Mapper.CreateMap<Address, AddressModel>();
            Mapper.CreateMap<AddressModel, Address>();

            Mapper.CreateMap<Season, SeasonModel>();
            Mapper.CreateMap<SeasonModel, Season>();
        }
    }
}