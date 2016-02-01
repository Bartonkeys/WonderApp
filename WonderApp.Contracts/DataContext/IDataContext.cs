using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using WonderApp.Data;

namespace WonderApp.Contracts.DataContext
{
    public interface IDataContext
    {
        IDbSet<Deal> Deals { get; }
        IDbSet<Location> Locations { get; }
        IDbSet<Tag> Tags { get; }
        IDbSet<Category> Categories { get; }
        IDbSet<Company> Companies { get; }
        IDbSet<Reminder> Reminders { get; }
        IDbSet<Cost> Costs { get; }
        IDbSet<Gender> Genders { get; }
        IDbSet<Country> Countries { get; }
        IDbSet<AspNetRole> AspNetRoles { get; }
        IDbSet<AspNetUserClaim> AspNetUserClaims { get; }
        IDbSet<AspNetUserLogin> AspNetUserLogins { get; }
        IDbSet<AspNetUser> AspNetUsers { get; }
        IDbSet<Device> Devices { get; }
        IDbSet<Image> Images { get; }
        IDbSet<City> Cities { get; }
        IDbSet<Season> Seasons { get; }
        IDbSet<Age> Ages { get; }
        IDbSet<Template> Templates { get; }
        IDbSet<NotificationEmail> NotificationEmails { get; }
        IDbSet<UserPreference> Preferences { get; }

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