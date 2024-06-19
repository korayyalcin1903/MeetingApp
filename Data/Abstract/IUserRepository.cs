using MeetingApp.Data.Entity;
using MeetingApp.Entity;

namespace MeetingApp.Data.Abstract
{
    public interface IUserRepository
    {
        IQueryable<User> Users { get; }

        void CreateUser(User user);
        void EditUser(User user, int id);
        void DeleteUser(User user);
    }
}