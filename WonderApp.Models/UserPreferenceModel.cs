using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WonderApp.Models
{
    public class UserPreferenceModel
    {
        public virtual ReminderModel Reminder { get; set; }

        public bool EmailMyWonders { get; set; }
    

    }
}
