using MeetingApp.Data.Entity;

namespace MeetingApp.Entity
{
    public class MeetingUser
    {
        public string UserId { get; set; }
        public User? User { get; set; }
        
        public int MeetingId { get; set; }
        public Meeting? Meeting { get; set; }

    }
}