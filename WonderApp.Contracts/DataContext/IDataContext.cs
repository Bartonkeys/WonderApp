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
        void Commit();
    }
}