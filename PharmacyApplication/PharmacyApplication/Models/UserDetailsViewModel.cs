using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmacyApplication.Models
{
    public class UserDetailsViewModel
    {
        public User CurrentUser { get; set; }
        public Pharmacist CurrentPharmacist { get; set; }

        public UserDetailsViewModel()
        {
            CurrentUser = new User();
            CurrentPharmacist = new Pharmacist();
        }
    }
}
