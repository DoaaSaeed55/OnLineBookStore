using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace OnLineBookStore.Models
{
    public class AppUser:IdentityUser
    {
        [PersonalData]
        public string FirstName { get; set; }
        [PersonalData]
        public string LastName { get; set; }
        [PersonalData]
        public string Address { get; set; }
        [PersonalData]
        public string ZipCode { get; set; }
        [PersonalData]
        public string City { get; set; }
        [PersonalData]
        [DataType(DataType.Date)]
        public DateTime UserCreationDate { get; set; }
    }
}
