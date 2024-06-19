using MeetingApp.Data.Concrete;
using MeetingApp.Data.Entity;
using MeetingApp.Entity;

namespace MeetingApp.Data.Abstract
{
    public class EFUserRepository : IUserRepository
    {
        private readonly MeetingContext _context;

        public EFUserRepository(MeetingContext context)
        {
            _context = context;
        }

        public IQueryable<User> Users => _context.Users;

        public void CreateUser(User User)
        {
            _context.Add(User);
            _context.SaveChanges();
        }

        public void DeleteUser(User User)
        {
            throw new NotImplementedException();
        }

        public void EditUser(User User, int id)
        {
            throw new NotImplementedException();
        }
    }
}