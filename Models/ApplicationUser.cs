using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Medical_clinic.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string SecondName { get; set; }
    }
}
