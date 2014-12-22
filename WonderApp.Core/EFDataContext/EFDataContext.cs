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

namespace WonderApp.Core.EFDataContext
{
    public class WonderAppContext : IDataContext
    {
        private readonly WonderAppModelContainer _context;
        public WonderAppContext()
        {
            _context = new WonderAppModelContainer();
        }

        public IDbSet<Deal> Deals { get { return _context.Deals; } }
        public IDbSet<Location> Locations { get { return _context.Locations; } }
        public IDbSet<Tag> Tags { get { return _context.Tags; } }
        public IDbSet<Category> Categories { get { return _context.Categories; } }
        public IDbSet<Company> Companies { get { return _context.Companies; } }
        public IDbSet<Reminder> Reminders { get { return _context.Reminders; } }
        public IDbSet<Cost> Costs { get { return _context.Costs; } }
        public IDbSet<Gender> Genders { get { return _context.Genders; } }
        public IDbSet<Country> Countries { get { return _context.Countries; } }
        public IDbSet<AspNetRole> AspNetRoles { get { return _context.AspNetRoles; } }
        public IDbSet<AspNetUserClaim> AspNetUserClaims { get { return _context.AspNetUserClaims; } }
        public IDbSet<AspNetUserLogin> AspNetUserLogins { get { return _context.AspNetUserLogins; } }
        public IDbSet<AspNetUser> AspNetUsers { get { return _context.AspNetUsers; } }
        public IDbSet<Device> Devices { get { return _context.Devices; } }
        public IDbSet<Image> Images { get { return _context.Images; } }
        public IDbSet<City> Cities { get { return _context.Cities; } }
        public IDbSet<Season> Seasons { get { return _context.Seasons; } }
        public IDbSet<Age> Ages { get { return _context.Ages; } }
        public IDbSet<NotificationEmail> NotificationEmails { get { return _context.NotificationEmails; } }
        public IDbSet<Template> Templates { get { return _context.Templates; } } 
       

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
    }
}
