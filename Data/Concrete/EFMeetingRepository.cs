using MeetingApp.Data.Abstract;
using MeetingApp.Entity;
using Microsoft.EntityFrameworkCore;

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
            _context.Remove(meeting);
            _context.SaveChanges();
        }

        public void EditMeeting(Meeting meeting)
        {
            var entity = _context.Meetings.FirstOrDefault(x => x.MeetingId == meeting.MeetingId);

            if( entity != null){
                entity.MeetingName = meeting.MeetingName;
                entity.Description = meeting.Description;
                entity.Location = meeting.Location;
                entity.Subject = meeting.Subject;
                entity.StartDate = meeting.StartDate;
                entity.MeetingPhoto = meeting.MeetingPhoto;
                
                _context.SaveChanges();
            }
        }
    }
}