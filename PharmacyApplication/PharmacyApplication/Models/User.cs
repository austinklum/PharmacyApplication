using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace PharmacyApplication.Models
{
    public class User
    {
        public int Id { get; set; }
        public String Username { get; set; }
        public String Password { get; set; }
        [DisplayName("Security Question 1")]
        public int SecQ1Index { get; set; }
        public String SecQ1Response { get; set; }
        [DisplayName("Security Question 2")]
        public int SecQ2Index { get; set; }
        public String SecQ2Response { get; set; }
        [DisplayName("Security Question 3")]
        public int SecQ3Index { get; set; }
        public String SecQ3Response { get; set; }
        public int AccountStatus { get; set; }
    }
}
