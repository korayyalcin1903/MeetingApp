using System.ComponentModel.DataAnnotations;

namespace MeetingApp.Models
{
    public class MeetingViewModel
    {
        [Required]
        [Display(Name = "Title")]
        public string MeetingName { get; set; }
        [Required]
        public string Subject { get; set; }
        [Required]
        [DataType(DataType.Html)]
        public string Description { get; set; }
        [Display(Name = "Meeting Photo")]
        public string MeetingPhoto { get; set; }
        [Required]
        public string Location { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Date")]
        public DateTime StartDate { get; set; }
    }
}