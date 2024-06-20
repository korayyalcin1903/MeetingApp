using MeetingApp.Data.Entity;
using MeetingApp.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MeetingApp.Data.Concrete
{
    public static class SeedData
    {
        public static void TestVerileriniDoldur(IApplicationBuilder app)
        {
            var context = app.ApplicationServices.CreateScope().ServiceProvider.GetService<MeetingContext>();

            if(context != null){
                context.Database.Migrate();
            }

            if(!context.Meetings.Any()){
                context.Meetings.AddRange(
                    new Meeting {MeetingName = "C# Programlama", Subject = "C#", Description = "Lorem ipsum dolor sit amet consectetur adipisicing elit. Sit eligendi, modi natus doloribus repellendus architecto sunt nostrum, eius veniam, expedita aspernatur optio cumque incidunt. Modi aliquid corrupti blanditiis sed cupiditate.", MeetingPhoto = "1.jpg", Location = "İstanbul", StartDate = DateTime.Now.AddDays(-10)},

                    new Meeting {MeetingName = "Python Programlama", Subject = "Python", Description = "Lorem ipsum dolor sit amet consectetur adipisicing elit. Sit eligendi, modi natus doloribus repellendus architecto sunt nostrum, eius veniam, expedita aspernatur optio cumque incidunt. Modi aliquid corrupti blanditiis sed cupiditate.", MeetingPhoto = "2.jpg", Location = "İstanbul", StartDate = DateTime.Now.AddDays(-5)}
                );
                context.SaveChanges();
            }

            if(!context.Users.Any()){
                context.Users.AddRange(
                    new IdentityUser { Id = "admin" , Name = "Koray", Surname = "Yalçın", UserName = "admin@gmail.com", Image = "user.jpg", Email = "admin@gmail.com", Password = "1234"}
                );
                context.SaveChanges();
            }

            if(!context.Roles.Any()){
                context.Roles.AddRange(
                    new IdentityRole { Id = "admin", Name = "Admin"}
                );
                context.SaveChanges();
            }

            if(!context.UserRoles.Any()){
                context.UserRoles.AddRange(
                    new IdentityUserRole { RoleId = "admin", UserId = "admin"}
                );
                context.SaveChanges();
            }

        }

    }
}