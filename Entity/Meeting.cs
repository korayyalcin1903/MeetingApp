using MeetingApp.Data.Entity;

namespace MeetingApp.Entity
{
    public class Meeting
    {
        public int MeetingId { get; set; }
        public string MeetingName { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public string MeetingPhoto { get; set; }
        public string Location { get; set; }
        public DateTime StartDate { get; set; }
        public IList<MeetingUser> MeetingUsers { get; set; } = new List<MeetingUser>();

    }
}