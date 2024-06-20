
using System.ComponentModel.DataAnnotations;
using MeetingApp.Entity;

namespace MeetingApp.Models
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }
}