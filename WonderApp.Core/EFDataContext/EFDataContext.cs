using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WonderApp.Contracts.DataContext;
using WonderApp.Data;
using WonderApp.Models;

namespace WonderApp.Core.EFDataContext
{
    public class WonderAppContext : IDataContext
    {
        private readonly WonderAppModelContainer _context;
        public WonderAppContext()
        {
            _context = new WonderAppModelContainer();
        }

        public void TurnOffLazyLoading()
        {
            _context.Configuration.LazyLoadingEnabled = false;
        }

        public void TurnOnLazyLoading()
        {
            _context.Configuration.LazyLoadingEnabled = true;
        }

        public DbSet<Deal> Deals { get { return _context.Deals; } }
        public DbSet<Location> Locations { get { return _context.Locations; } }
        public DbSet<Tag> Tags { get { return _context.Tags; } }
        public DbSet<Category> Categories { get { return _context.Categories; } }
        public DbSet<Company> Companies { get { return _context.Companies; } }
        public DbSet<Reminder> Reminders { get { return _context.Reminders; } }
        public DbSet<Cost> Costs { get { return _context.Costs; } }
        public DbSet<Gender> Genders { get { return _context.Genders; } }
        public DbSet<Country> Countries { get { return _context.Countries; } }
        public DbSet<AspNetRole> AspNetRoles { get { return _context.AspNetRoles; } }
        public DbSet<AspNetUserClaim> AspNetUserClaims { get { return _context.AspNetUserClaims; } }
        public DbSet<AspNetUserLogin> AspNetUserLogins { get { return _context.AspNetUserLogins; } }
        public DbSet<AspNetUser> AspNetUsers { get { return _context.AspNetUsers; } }
        public DbSet<Device> Devices { get { return _context.Devices; } }
        public DbSet<Image> Images { get { return _context.Images; } }
        public DbSet<City> Cities { get { return _context.Cities; } }
        public DbSet<Season> Seasons { get { return _context.Seasons; } }
        public DbSet<Age> Ages { get { return _context.Ages; } }
        public DbSet<NotificationEmail> NotificationEmails { get { return _context.NotificationEmails; } }
        public DbSet<Template> Templates { get { return _context.Templates; } }
        public DbSet<UserPreference> Preferences { get { return _context.UserPreferences; }}

        public List<GetWonders_Result> GetWonders(string userId, int cityId, bool priority)
        {
            return _context.GetWonders(userId, cityId, priority ? 1 : 0).ToList();
        }

        public List<GetWonders_Result> GetMyWonders(string userId)
        {
            return _context.GetMyWonders(userId).ToList();
        }

        public List<GetTags_Result> GetTags(string userId, int cityId, bool priority)
        {
            return _context.GetTags(userId, cityId, priority ? 1 : 0).ToList();
        }

        public List<GetAges_Result> GetAges(string userId, int cityId, bool priority)
        {
            return _context.GetAges(userId, cityId, priority ? 1 : 0).ToList();
        }

        public List<GetTags_Result> GetWonderTags(string userId)
        {
            return _context.GetWonderTags(userId).ToList();
        }

        public List<GetAges_Result> GetWonderAges(string userId)
        {
            return _context.GetWonderAges(userId).ToList();
        }

        public GetWonders_Result GetWonder(int wonderId)
        {
            return _context.GetWonder(wonderId).SingleOrDefault();
        }

        public void Commit()
        {
            try
            {
                _context.SaveChanges();
            }
            catch (DbEntityValidationException exc)
            {
                IEnumerable<DbValidationError> dbValidationErrors =
                    exc.EntityValidationErrors.SelectMany(error => error.ValidationErrors);

                foreach (var error in dbValidationErrors)
                {
                   
                }
            }
            catch (Exception exc)
            {
                Debug.Write(exc.Message);
            }
        }


        public List<GetTags_Result> GetWonderTags(int wonderId)
        {
            return _context.GetTagsForWonder(wonderId).ToList();
        }

        public List<GetAges_Result> GetWonderAges(int wonderId)
        {
            return _context.GetAgesForWonder(wonderId).ToList();
        }
    }
}
