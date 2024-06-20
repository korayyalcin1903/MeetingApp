
using System.ComponentModel.DataAnnotations;
using MeetingApp.Entity;

namespace MeetingApp.Models
{
    public class RegisterViewModel
    {
        public int UserId { get; set; }
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Surname")]
        public string Surname { get; set; }
        public string Image { get; set; } = "user.jpg";
        [Required]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Parola eşleşmiyor")]
        public string ConfirmPassword { get; set; }
        public List<Meeting> Meetings { get; set; } = new List<Meeting>();

    }
}