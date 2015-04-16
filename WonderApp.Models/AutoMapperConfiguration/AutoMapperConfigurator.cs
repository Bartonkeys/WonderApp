using System;
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
                .ForMember(m => m.ExpiryDate,
                    opt => opt.MapFrom(e => e.AlwaysAvailable == true
                        ? String.Empty : e.ExpiryDate.ToString("dd-MM-yyyy HH:mm:ss")))
                .ForMember(e => e.Phone, opt => opt.NullSubstitute(String.Empty))
                //.ForMember(dest => dest.Category.Deals, opt => opt.Ignore()) 
                //.ForMember(dest => dest.Expired, opts => opts.MapFrom(src => src.Expired == null ? src.Expired.Value : false))
                // .ForMember(dest => dest.Archived, opts => opts.MapFrom(src => src.Archived == null ? src.Archived.Value : false))
               //.ForMember(dest => dest.AlwaysAvailable,  opt => opt.Ignore()) //opts => opts.MapFrom(src => src.AlwaysAvailable == null ? src.AlwaysAvailable.Value : false));
               .ForMember(dest => dest.Creator, opts => opts.MapFrom(src => src.AspNetUser));

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
                .ForMember(e => e.Creator_User_Id, opt => opt.Ignore())
                .ForMember(e => e.Season, opt => opt.Ignore())
                .ForMember(e => e.Gender, opt => opt.Ignore())
                .ForMember(e => e.Ages, opt => opt.Ignore())
                .ForMember(e => e.Likes, opt => opt.Ignore())
                .ForMember(e => e.ExpiryDate, opt => opt.MapFrom(m => m.AlwaysAvailable == true ? DateTime.Now.ToShortDateString() : m.ExpiryDate))
                .ForAllMembers(opt => opt.Condition(srs => !srs.IsSourceValueNull));

            Mapper.CreateMap<GetWonders_Result, DealModel>()
                .ForMember(e => e.Company, opt => opt.MapFrom(m => new CompanyModel { Id = m.Company_Id, Name = m.CompanyName }))
                .ForMember(e => e.Location, opt => opt.MapFrom(m => new LocationModel { Id = m.Location_Id, Latitude = m.Latitude, Longitude = m.Longitude  }))
                .ForMember(e => e.Cost, opt => opt.MapFrom(m => new CostModel { Id = m.Cost_Id.Value, Range = m.Range }))
                .ForMember(e => e.Category, opt => opt.MapFrom(m => new CategoryModel { Id = m.Category_Id.Value, Name = m.CategoryName }))
                .ForMember(e => e.Images, opt => opt.ResolveUsing(m => 
                {
                    var imageList = new List<ImageModel>();
                    var image = new ImageModel { url = m.ImageURL };
                    imageList.Add(image);
                    return imageList;
                }))
                .ForMember(e => e.City, opt => opt.MapFrom(m => new CityModel { Id = m.CityId, Name = m.CityName }))
                .ForMember(e => e.Address, opt => opt.MapFrom(m => new AddressModel { Id = m.AddressId.ToString(), AddressLine1 = m.AddressLine1, AddressLine2 = m.AddressLine2, PostCode = m.PostCode}))
                .ForMember(e => e.Season, opt => opt.MapFrom(m => new SeasonModel { Id = m.Season_Id.Value, Name = m.Season }))
                .ForMember(e => e.Gender, opt => opt.MapFrom(m => new GenderModel { Id = m.Gender_Id.Value, Name = m.Gender }))
                .ForMember(e => e.Creator, opt => opt.Ignore())
                .ForMember(e => e.Ages, opt => opt.Ignore())
                .ForMember(e => e.Tags, opt => opt.Ignore());

            Mapper.CreateMap<Deal, DealSummaryModel>()
             .ForMember(dest => dest.Creator,
                           opts => opts.MapFrom(src => src.AspNetUser));
                //.ForMember(m => m.ExpiryDate, opt => opt.ResolveUsing(e => e.ExpiryDate.MapToString()));
           

            Mapper.CreateMap<Category, CategoryModel>()
                .ForMember(e => e.Deals, opt => opt.Ignore())
                .ForMember(dest => dest.Users, opt => opt.NullSubstitute(new List<UserModel>()));
            Mapper.CreateMap<CategoryModel, Category>()
                .ForMember(e => e.Deals, opt => opt.Ignore())
                .ForMember(e => e.Users, opt => opt.Ignore());

            Mapper.CreateMap<Company, CompanyModel>()
                .ForMember(e => e.Deals, opt => opt.Ignore());
            Mapper.CreateMap<CompanyModel, Company>()
                .ForMember(e => e.Country, opt => opt.Ignore())
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
                .ForMember(e => e.Cities, opt => opt.Ignore())
                .ForMember(e => e.AspNetUser, opt => opt.Ignore());

            Mapper.CreateMap<Tag, TagModel>()
                .ForMember(e => e.Deals, opt => opt.Ignore());
            Mapper.CreateMap<GetTags_Result, TagModel>()
                .ForMember(e => e.Id, opt => opt.MapFrom(m => m.TagId))
                .ForMember(e => e.Deals, opt => opt.Ignore())
                .ForMember(e => e.Name, opt => opt.MapFrom(m => m.TagName));
            Mapper.CreateMap<TagModel, Tag>()
                .ForMember(e => e.Deals, opt => opt.Ignore());

            Mapper.CreateMap<AspNetUser, UserModel>()
                .ForMember(e => e.MyCategories, opt => opt.Ignore());

            Mapper.CreateMap<UserModel, AspNetUser>()
                .ForMember(e => e.EmailConfirmed, opt => opt.Ignore())
                .ForMember(e => e.PasswordHash, opt => opt.Ignore())
                .ForMember(e => e.SecurityStamp, opt => opt.Ignore())
                .ForMember(e => e.PhoneNumberConfirmed, opt => opt.Ignore())
                .ForMember(e => e.TwoFactorEnabled, opt => opt.Ignore())
                .ForMember(e => e.LockoutEndDateUtc, opt => opt.Ignore())
                .ForMember(e => e.LockoutEnabled, opt => opt.Ignore())
                .ForMember(e => e.AccessFailedCount, opt => opt.Ignore())
                .ForMember(e => e.Name, opt => opt.Ignore())
                .ForMember(e => e.AspNetUserClaims, opt => opt.Ignore())
                .ForMember(e => e.Roles, opt => opt.Ignore())
                .ForMember(e => e.Categories, opt => opt.Ignore())
                .ForMember(e => e.Deals, opt => opt.Ignore());

            Mapper.CreateMap<AspNetUser, UserBasicModel>();

            Mapper.CreateMap<UserBasicModel, AspNetUser>()
                .ForMember(e => e.EmailConfirmed, opt => opt.Ignore())
                .ForMember(e => e.PasswordHash, opt => opt.Ignore())
                .ForMember(e => e.SecurityStamp, opt => opt.Ignore())
                .ForMember(e => e.PhoneNumberConfirmed, opt => opt.Ignore())
                .ForMember(e => e.TwoFactorEnabled, opt => opt.Ignore())
                .ForMember(e => e.LockoutEndDateUtc, opt => opt.Ignore())
                .ForMember(e => e.LockoutEnabled, opt => opt.Ignore())
                .ForMember(e => e.AccessFailedCount, opt => opt.Ignore())
                .ForMember(e => e.Name, opt => opt.Ignore())
                .ForMember(e => e.AspNetUserClaims, opt => opt.Ignore())
                .ForMember(e => e.AspNetUserLogins, opt => opt.Ignore())
                .ForMember(e => e.Roles, opt => opt.Ignore())
                .ForMember(e => e.Categories, opt => opt.Ignore())
                .ForMember(e => e.UserPreference, opt => opt.Ignore())
                .ForMember(e => e.MyWonders, opt => opt.Ignore())
                .ForMember(e => e.MyRejects, opt => opt.Ignore())
                .ForMember(e => e.Locations, opt => opt.Ignore())
                .ForMember(e => e.Gender, opt => opt.Ignore())
                .ForMember(e => e.Deals, opt => opt.Ignore());

            Mapper.CreateMap<AspNetUserLogin, AspNetUserLoginModel>();
            Mapper.CreateMap<AspNetUserLoginModel, AspNetUserLogin>()
                .ForMember(e => e.AspNetUser, opt => opt.Ignore());

            Mapper.CreateMap<Reminder, ReminderModel>();
            Mapper.CreateMap<ReminderModel, Reminder>()
                .ForMember(e => e.UserPreferences, opt => opt.Ignore());

            Mapper.CreateMap<Gender, GenderModel>()
                .ForMember(e => e.Deals, opt => opt.Ignore());
            Mapper.CreateMap<GenderModel, Gender>()
                  .ForMember(e => e.AspNetUsers, opt => opt.Ignore());

            Mapper.CreateMap<Image, ImageModel>()
                .ForMember(e => e.Deal, opt => opt.Ignore());
            Mapper.CreateMap<ImageModel, Image>();

            Mapper.CreateMap<Device, DeviceModel>()
                .ForMember(e => e.Images, opt => opt.Ignore());
            Mapper.CreateMap<DeviceModel, Device>();

            Mapper.CreateMap<City, CityModel>();
            Mapper.CreateMap<CityModel, City>()
                .ForMember(e => e.Companies, opt => opt.Ignore())
                .ForMember(e => e.Deals, opt => opt.Ignore());

            Mapper.CreateMap<Address, AddressModel>()
                .ForMember(e => e.AddressLine2, opt => opt.NullSubstitute(String.Empty));
            Mapper.CreateMap<AddressModel, Address>()
                .ForMember(e => e.Deals, opt => opt.Ignore());

            Mapper.CreateMap<Season, SeasonModel>()
                .ForMember(e => e.Deals, opt => opt.Ignore());
            Mapper.CreateMap<SeasonModel, Season>();

            Mapper.CreateMap<Age, AgeModel>()
                .ForMember(e => e.Deals, opt => opt.Ignore());
            Mapper.CreateMap<GetAges_Result, AgeModel>()
                .ForMember(e => e.Id, opt => opt.MapFrom(m => m.AgeId))
                .ForMember(e => e.Name, opt => opt.MapFrom(m => m.AgeName))
                .ForMember(e => e.Deals, opt => opt.Ignore());
            Mapper.CreateMap<AgeModel, Age>()
                .ForMember(e => e.Deals, opt => opt.Ignore());

            Mapper.CreateMap<UserPreference, UserPreferenceModel>();
            Mapper.CreateMap<UserPreferenceModel, UserPreference>()
                .ForMember(e => e.UserId, opt => opt.Ignore())
                .ForMember(e => e.AspNetUser, opt => opt.Ignore());

            Mapper.CreateMap<AspNetUser, UserInfoModel>()
                .ForMember(e => e.MyCategories, opt => opt.Ignore());

            Mapper.CreateMap<UserInfoModel, AspNetUser>()
                .ForMember(e => e.EmailConfirmed, opt => opt.Ignore())
                .ForMember(e => e.PasswordHash, opt => opt.Ignore())
                .ForMember(e => e.SecurityStamp, opt => opt.Ignore())
                .ForMember(e => e.PhoneNumberConfirmed, opt => opt.Ignore())
                .ForMember(e => e.TwoFactorEnabled, opt => opt.Ignore())
                .ForMember(e => e.LockoutEndDateUtc, opt => opt.Ignore())
                .ForMember(e => e.LockoutEnabled, opt => opt.Ignore())
                .ForMember(e => e.AccessFailedCount, opt => opt.Ignore())
                .ForMember(e => e.Name, opt => opt.Ignore())
                .ForMember(e => e.AspNetUserClaims, opt => opt.Ignore())
                .ForMember(e => e.AspNetUserLogins, opt => opt.Ignore())
                .ForMember(e => e.Roles, opt => opt.Ignore())
                .ForMember(e => e.Categories, opt => opt.Ignore())
                .ForMember(e => e.UserPreference, opt => opt.Ignore())
                .ForMember(e => e.MyWonders, opt => opt.Ignore())
                .ForMember(e => e.MyRejects, opt => opt.Ignore())
                .ForMember(e => e.Locations, opt => opt.Ignore())
                .ForMember(e => e.Deals, opt => opt.Ignore())
                .ForMember(e => e.Gender, opt => opt.Ignore())
                .ForMember(e => e.PhoneNumber, opt => opt.Ignore())
                .ForMember(e => e.UserName, opt => opt.Ignore())
                .ForMember(e => e.Forename, opt => opt.Ignore())
                .ForMember(e => e.Surname, opt => opt.Ignore());

            Mapper.CreateMap<Template, TemplateModel>();
            Mapper.CreateMap<TemplateModel, Template>()
                .ForMember(e => e.NotificationEmails, opt => opt.Ignore());

            Mapper.CreateMap<NotificationEmail, NotificationEmailModel>()
                .ForMember(e => e.Recipients, opt => opt.Ignore());
            Mapper.CreateMap<NotificationEmailModel, NotificationEmail>()
                .ForMember(e => e.Template, opt => opt.Ignore())
                .ForMember(e => e.RecipientEmail, opt => opt.Ignore())
                .ForMember(e => e.RecipientName, opt => opt.Ignore())
                .ForMember(e => e.Template_Id, opt => opt.Ignore());

           Mapper.AssertConfigurationIsValid();

        }
    }
}