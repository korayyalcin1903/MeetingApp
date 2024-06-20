using MeetingApp.Entity;
using Microsoft.AspNetCore.Identity;

namespace MeetingApp.Data.Entity
{
    public class User: IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Image { get; set; } =  "user.jpg";
        public List<MeetingUser> MeetingUsers { get; set; } = new List<MeetingUser>();

    }
}