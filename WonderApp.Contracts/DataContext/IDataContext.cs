using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using WonderApp.Data;

namespace WonderApp.Contracts.DataContext
{
    public interface IDataContext
    {
        DbSet<Deal> Deals { get; }
        DbSet<Location> Locations { get; }
        DbSet<Tag> Tags { get; }
        DbSet<Category> Categories { get; }
        DbSet<Company> Companies { get; }
        DbSet<Reminder> Reminders { get; }
        DbSet<Cost> Costs { get; }
        DbSet<Gender> Genders { get; }
        DbSet<Country> Countries { get; }
        DbSet<AspNetRole> AspNetRoles { get; }
        DbSet<AspNetUserClaim> AspNetUserClaims { get; }
        DbSet<AspNetUserLogin> AspNetUserLogins { get; }
        DbSet<AspNetUser> AspNetUsers { get; }
        DbSet<Device> Devices { get; }
        DbSet<Image> Images { get; }
        DbSet<City> Cities { get; }
        DbSet<Season> Seasons { get; }
        DbSet<Age> Ages { get; }
        DbSet<Template> Templates { get; }
        DbSet<NotificationEmail> NotificationEmails { get; }
        DbSet<UserPreference> Preferences { get; }

        List<GetWonders_Result> GetWonders(string userId, int cityId, bool priority);
        List<GetWonders_Result> GetMyWonders(string userId);
        List<GetTags_Result> GetTags(string userId, int cityId, bool priority);
        List<GetAges_Result> GetAges(string userId, int cityId, bool priority);
        List<GetTags_Result> GetWonderTags(string userId);
        List<GetAges_Result> GetWonderAges(string userId);
        List<GetTags_Result> GetWonderTags(int wonderId);
        List<GetAges_Result> GetWonderAges(int wonderId);
        GetWonders_Result GetWonder(int wonderId);
       
        void Commit();

        void TurnOffLazyLoading();
        void TurnOnLazyLoading();
    }
}