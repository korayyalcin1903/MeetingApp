using MeetingApp.Entity;

namespace MeetingApp.Data.Abstract
{
    public interface IMeetingRepository
    {
        IQueryable<Meeting> Meetings { get; }

        void CreateMeeting(Meeting meeting);
        void EditMeeting(Meeting meeting);
        void DeleteMeeting(Meeting meeting);
    }
}