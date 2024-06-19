using MeetingApp.Data.Abstract;
using MeetingApp.Entity;

namespace MeetingApp.Data.Concrete
{
    public class EFMeetingRepository : IMeetingRepository
    {
        private readonly MeetingContext _context;

        public EFMeetingRepository(MeetingContext context)
        {
            _context = context;
        }

        public IQueryable<Meeting> Meetings => _context.Meetings;

        public void CreateMeeting(Meeting meeting)
        {
            
        }

        public void DeleteMeeting(Meeting meeting)
        {
            
        }

        public void EditMeeting(Meeting meeting, int id)
        {
            
        }
    }
}