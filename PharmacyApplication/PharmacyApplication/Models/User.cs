using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PharmacyApplication.Models
{
    public class User
    {
        public int Id { get; set; }
        public String Username { get; set; }
        public byte[] PasswordHash { get; set; }
        [DisplayName("Security Question 1")]
        public int SecQ1Index { get; set; }
        public byte[] SecQ1ResponseHash { get; set; }
        [DisplayName("Security Question 2")]
        public int SecQ2Index { get; set; }
        public byte[] SecQ2ResponseHash { get; set; }
        [DisplayName("Security Question 3")]
        public int SecQ3Index { get; set; }
        public byte[] SecQ3ResponseHash { get; set; }
        public int AccountStatus { get; set; }
        public byte[] Salt { get; set; }

        [NotMapped]
        [StringLength(50, MinimumLength = 6), RegularExpression(@"[a-zA-Z0-9~!@#$%^&*+]*[~!@#$%^&*+]+[a-zA-Z0-9~!@#$%^&*+]*", ErrorMessage = "Password must be 6-50 letters long with only letters, numbers, and at least one special character (~,!,@,#,$,%,^,&,*,+) and no spaces")]
        public string Password { get; set; }
        [NotMapped]
        [RegularExpression(@"[a-zA-Z0-9]{4,50}", ErrorMessage = "Entry must be a single word 4-50 letters long with only letters and numbers and no spaces")]
        [DisplayName("Response 1")]
        public String SecQ1Response { get; set; }
        [NotMapped]
        [RegularExpression(@"[a-zA-Z0-9]{4,50}", ErrorMessage = "Entry must be a single word 4-50 letters long with only letters and numbers and no spaces")]
        [DisplayName("Response 2")]
        public String SecQ2Response { get; set; }
        [NotMapped]
        [RegularExpression(@"[a-zA-Z0-9]{4,50}", ErrorMessage = "Entry must be a single word 4-50 letters long with only letters and numbers and no spaces")]
        [DisplayName("Response 3")]
        public String SecQ3Response { get; set; }
    }
}
